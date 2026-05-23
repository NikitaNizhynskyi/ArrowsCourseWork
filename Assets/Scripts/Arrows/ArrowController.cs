using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public string ArrowId { get; private set; }
    public ArrowDirection Direction { get; private set; }

    [Header("Movement")]
    [SerializeField] private float stepDuration = 0.15f;
    [SerializeField] private float pauseBetweenSteps = 0.03f;

    [Header("Error Visual")]
    [SerializeField] private Color collisionColor = new Color32(220, 55, 55, 255);

    private readonly List<Vector2Int> cells = new List<Vector2Int>();
    private readonly List<GameObject> parts = new List<GameObject>();
    private readonly List<SpriteRenderer> partRenderers = new List<SpriteRenderer>();
    private readonly List<Color> originalColors = new List<Color>();

    private GridManager gridManager;
    private LevelController levelController;

    private float cellSize;
    private Vector2 gridOrigin;

    private bool isMoving;
    private bool isMarkedAsError;

    public void Initialize(
        ArrowData data,
        List<GameObject> createdParts,
        GridManager grid,
        LevelController controller,
        float size,
        Vector2 origin)
    {
        ArrowId = data.arrowId;
        Direction = data.direction;

        cells.Clear();
        cells.AddRange(data.cells);

        parts.Clear();
        parts.AddRange(createdParts);

        gridManager = grid;
        levelController = controller;
        cellSize = size;
        gridOrigin = origin;

        CacheRenderersAndColors();

        Debug.Log("Arrow initialized: " + ArrowId + ", Direction: " + Direction);
    }

    public void OnClicked()
    {
        if (isMoving)
        {
            return;
        }

        if (levelController == null || !levelController.CanUseGameInput)
        {
            return;
        }

        ClearErrorColor();

        Debug.Log("Arrow clicked: " + ArrowId);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayArrowClick();
        }

        StartCoroutine(MoveUntilStop());
    }

    private IEnumerator MoveUntilStop()
    {
        isMoving = true;

        List<Vector2Int> startCells = new List<Vector2Int>(cells);
        List<List<Vector3>> positionHistory = new List<List<Vector3>>
        {
            GetCurrentPartPositions()
        };

        while (true)
        {
            Vector2Int directionVector = GetDirectionVector(Direction);
            Vector2Int currentHeadCell = cells[cells.Count - 1];
            Vector2Int nextHeadCell = currentHeadCell + directionVector;

            if (!gridManager.IsOutsideGrid(nextHeadCell) &&
                gridManager.IsOccupiedByOtherArrow(nextHeadCell, ArrowId))
            {
                Debug.Log("Collision detected: " + ArrowId);

                levelController.OnArrowCollision();

                if (levelController.IsLevelPlaying)
                {
                    yield return MoveBackToStart(positionHistory, startCells);
                    SetErrorColor();
                }

                isMoving = false;
                yield break;
            }

            List<Vector2Int> oldCells = new List<Vector2Int>(cells);
            List<Vector2Int> newCells = BuildNextCells(nextHeadCell);

            FreeCells(oldCells);
            RegisterCells(newCells);

            List<Vector3> startPositions = GetCurrentPartPositions();
            List<Vector3> targetPositions = GetWorldPositions(newCells);

            yield return MovePartsSmoothly(startPositions, targetPositions);

            cells.Clear();
            cells.AddRange(newCells);

            positionHistory.Add(GetCurrentPartPositions());

            if (IsArrowFullyOutsideGrid())
            {
                Debug.Log("Arrow escaped: " + ArrowId);

                levelController.OnArrowEscaped();

                Destroy(gameObject);
                yield break;
            }

            yield return new WaitForSeconds(pauseBetweenSteps);
        }
    }

    private List<Vector2Int> BuildNextCells(Vector2Int nextHeadCell)
    {
        List<Vector2Int> newCells = new List<Vector2Int>();

        for (int i = 1; i < cells.Count; i++)
        {
            newCells.Add(cells[i]);
        }

        newCells.Add(nextHeadCell);

        return newCells;
    }

    private IEnumerator MoveBackToStart(List<List<Vector3>> positionHistory, List<Vector2Int> startCells)
    {
        for (int i = positionHistory.Count - 2; i >= 0; i--)
        {
            List<Vector3> currentPositions = GetCurrentPartPositions();
            List<Vector3> targetPositions = positionHistory[i];

            yield return MovePartsSmoothly(currentPositions, targetPositions);
        }

        FreeCells(cells);
        RegisterCells(startCells);

        cells.Clear();
        cells.AddRange(startCells);
    }

    private IEnumerator MovePartsSmoothly(List<Vector3> startPositions, List<Vector3> targetPositions)
    {
        float timer = 0f;

        while (timer < stepDuration)
        {
            timer += Time.deltaTime;
            float t = timer / stepDuration;

            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].transform.position = Vector3.Lerp(
                    startPositions[i],
                    targetPositions[i],
                    t
                );
            }

            yield return null;
        }

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].transform.position = targetPositions[i];
        }
    }

    private void FreeCells(List<Vector2Int> cellsToFree)
    {
        foreach (Vector2Int cell in cellsToFree)
        {
            if (!gridManager.IsOutsideGrid(cell))
            {
                gridManager.FreeCell(cell);
            }
        }
    }

    private void RegisterCells(List<Vector2Int> cellsToRegister)
    {
        foreach (Vector2Int cell in cellsToRegister)
        {
            if (!gridManager.IsOutsideGrid(cell))
            {
                gridManager.RegisterCell(cell, ArrowId);
            }
        }
    }

    private List<Vector3> GetCurrentPartPositions()
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (GameObject part in parts)
        {
            positions.Add(part.transform.position);
        }

        return positions;
    }

    private List<Vector3> GetWorldPositions(List<Vector2Int> cellPositions)
    {
        List<Vector3> positions = new List<Vector3>();

        foreach (Vector2Int cell in cellPositions)
        {
            positions.Add(CellToWorldPosition(cell));
        }

        return positions;
    }

    private Vector3 CellToWorldPosition(Vector2Int cell)
    {
        float x = gridOrigin.x + cell.x * cellSize;
        float y = gridOrigin.y + cell.y * cellSize;

        return new Vector3(x, y, 0f);
    }

    private Vector2Int GetDirectionVector(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Up:
                return Vector2Int.up;

            case ArrowDirection.Down:
                return Vector2Int.down;

            case ArrowDirection.Left:
                return Vector2Int.left;

            case ArrowDirection.Right:
                return Vector2Int.right;

            default:
                return Vector2Int.zero;
        }
    }

    private bool IsArrowFullyOutsideGrid()
    {
        foreach (Vector2Int cell in cells)
        {
            if (!gridManager.IsOutsideGrid(cell))
            {
                return false;
            }
        }

        return true;
    }

    private void CacheRenderersAndColors()
    {
        partRenderers.Clear();
        originalColors.Clear();

        foreach (GameObject part in parts)
        {
            SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                continue;
            }

            partRenderers.Add(spriteRenderer);
            originalColors.Add(spriteRenderer.color);
        }
    }

    private void SetErrorColor()
    {
        isMarkedAsError = true;

        foreach (SpriteRenderer spriteRenderer in partRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = collisionColor;
            }
        }
    }

    private void ClearErrorColor()
    {
        if (!isMarkedAsError)
        {
            return;
        }

        for (int i = 0; i < partRenderers.Count; i++)
        {
            if (partRenderers[i] != null)
            {
                partRenderers[i].color = originalColors[i];
            }
        }

        isMarkedAsError = false;
    }
}