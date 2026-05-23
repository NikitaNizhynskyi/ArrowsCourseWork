using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private LevelData levelData;
    [SerializeField] private List<LevelData> availableLevels = new List<LevelData>();
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelController levelController;
    [SerializeField] private LevelCameraFitter cameraFitter;

    [Header("Visual")]
    [SerializeField] private Camera levelCamera;
    [SerializeField] private SpriteRenderer levelBackgroundRenderer;
    [SerializeField] private LevelGridVisual gridVisual;

    [Header("Prefabs")]
    [SerializeField] private GameObject arrowHeadPrefab;
    [SerializeField] private GameObject arrowSegmentPrefab;

    [Header("Grid Settings")]
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;

    private List<GameObject> spawnedArrows = new List<GameObject>();

    private void Start()
    {
        SelectLevelFromSave();
        SpawnLevel();
    }

    private void SelectLevelFromSave()
    {
        int selectedLevelId = SaveManager.GetSelectedLevelId();

        foreach (LevelData level in availableLevels)
        {
            if (level != null && level.levelId == selectedLevelId)
            {
                levelData = level;
                Debug.Log("Loaded selected level: " + selectedLevelId);
                return;
            }
        }

        Debug.LogWarning("Selected level not found. Using default LevelData.");
    }

    public void SpawnLevel()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned in LevelSpawner");
            return;
        }

        if (gridManager == null)
        {
            Debug.LogError("GridManager is not assigned in LevelSpawner");
            return;
        }

        if (levelController == null)
        {
            Debug.LogError("LevelController is not assigned in LevelSpawner");
            return;
        }

        if (arrowHeadPrefab == null)
        {
            Debug.LogError("ArrowHeadPrefab is not assigned in LevelSpawner");
            return;
        }

        if (arrowSegmentPrefab == null)
        {
            Debug.LogError("ArrowSegmentPrefab is not assigned in LevelSpawner");
            return;
        }

        ClearLevel();

        gridManager.SetGridSize(levelData.gridSize);

        if (cameraFitter != null)
        {
            cameraFitter.FitToLevel(levelData.gridSize, cellSize, gridOrigin);
        }

        ApplyLevelTheme();

        if (gridVisual != null)
        {
            gridVisual.Build(levelData.gridSize, cellSize, gridOrigin, levelData.gridColor);
        }

        levelController.InitializeLevel(levelData.levelId, levelData.arrows.Count);

        foreach (ArrowData arrowData in levelData.arrows)
        {
            SpawnArrow(arrowData);
        }

        Debug.Log("Level spawned. Level ID: " + levelData.levelId);
        Debug.Log("Arrows count: " + levelData.arrows.Count);
    }

    public void RestartLevel()
    {
        SpawnLevel();
    }

    private void SpawnArrow(ArrowData arrowData)
    {
        if (arrowData.cells == null || arrowData.cells.Count == 0)
        {
            Debug.LogWarning("Arrow has no cells: " + arrowData.arrowId);
            return;
        }

        GameObject arrowObject = new GameObject(arrowData.arrowId);
        arrowObject.transform.SetParent(transform);

        spawnedArrows.Add(arrowObject);

        ArrowController arrowController = arrowObject.AddComponent<ArrowController>();

        List<GameObject> createdParts = new List<GameObject>();

        for (int i = 0; i < arrowData.cells.Count; i++)
        {
            Vector2Int cell = arrowData.cells[i];

            bool isHead = i == arrowData.cells.Count - 1;

            GameObject prefabToSpawn = isHead ? arrowHeadPrefab : arrowSegmentPrefab;

            Vector3 worldPosition = CellToWorldPosition(cell);

            GameObject part = Instantiate(
                prefabToSpawn,
                worldPosition,
                GetRotationByDirection(arrowData.direction),
                arrowObject.transform
            );

            ApplyArrowPartColor(part, isHead);

            if (part.GetComponent<Collider2D>() == null)
            {
                part.AddComponent<BoxCollider2D>();
            }

            ArrowPartClickHandler clickHandler = part.GetComponent<ArrowPartClickHandler>();

            if (clickHandler == null)
            {
                clickHandler = part.AddComponent<ArrowPartClickHandler>();
            }

            clickHandler.Initialize(arrowController);

            if (isHead)
            {
                part.name = arrowData.arrowId + "_Head";
            }
            else
            {
                part.name = arrowData.arrowId + "_Segment_" + i;
            }

            createdParts.Add(part);

            gridManager.RegisterCell(cell, arrowData.arrowId);
        }

        arrowController.Initialize(
            arrowData,
            createdParts,
            gridManager,
            levelController,
            cellSize,
            gridOrigin
        );
    }

    private void ApplyLevelTheme()
    {
        if (levelCamera != null)
        {
            levelCamera.backgroundColor = levelData.backgroundColor;
        }

        if (levelBackgroundRenderer != null)
        {
            levelBackgroundRenderer.color = levelData.backgroundColor;
        }
    }

    private void ApplyArrowPartColor(GameObject part, bool isHead)
    {
        SpriteRenderer spriteRenderer = part.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.color = isHead
            ? levelData.arrowHeadColor
            : levelData.arrowSegmentColor;
    }

    private Vector3 CellToWorldPosition(Vector2Int cell)
    {
        float x = gridOrigin.x + cell.x * cellSize;
        float y = gridOrigin.y + cell.y * cellSize;

        return new Vector3(x, y, 0f);
    }

    private Quaternion GetRotationByDirection(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Up:
                return Quaternion.Euler(0f, 0f, 90f);

            case ArrowDirection.Down:
                return Quaternion.Euler(0f, 0f, -90f);

            case ArrowDirection.Left:
                return Quaternion.Euler(0f, 0f, 180f);

            case ArrowDirection.Right:
                return Quaternion.Euler(0f, 0f, 0f);

            default:
                return Quaternion.identity;
        }
    }

    private void ClearLevel()
    {
        foreach (GameObject arrow in spawnedArrows)
        {
            if (arrow != null)
            {
                Destroy(arrow);
            }
        }

        spawnedArrows.Clear();

        if (gridVisual != null)
        {
            gridVisual.Clear();
        }

        gridManager.ClearGrid();
    }
}