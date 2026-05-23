using System.Collections.Generic;
using UnityEngine;

public class LevelGridVisual : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Transform linesParent;
    [SerializeField] private float lineThickness = 0.03f;

    private readonly List<GameObject> spawnedLines = new List<GameObject>();

    public void Build(Vector2Int gridSize, float cellSize, Vector2 gridOrigin, Color gridColor)
    {
        Clear();

        if (linePrefab == null)
        {
            Debug.LogWarning("Line prefab is not assigned in LevelGridVisual");
            return;
        }

        Transform parent = linesParent != null ? linesParent : transform;

        float minX = gridOrigin.x - cellSize / 2f;
        float minY = gridOrigin.y - cellSize / 2f;

        float width = gridSize.x * cellSize;
        float height = gridSize.y * cellSize;

        float centerX = minX + width / 2f;
        float centerY = minY + height / 2f;

        for (int x = 0; x <= gridSize.x; x++)
        {
            float lineX = minX + x * cellSize;

            GameObject line = Instantiate(linePrefab, parent);
            line.name = "Vertical_Line_" + x;
            line.transform.position = new Vector3(lineX, centerY, 0f);
            line.transform.localScale = new Vector3(lineThickness, height, 1f);

            ApplyColor(line, gridColor);
            spawnedLines.Add(line);
        }

        for (int y = 0; y <= gridSize.y; y++)
        {
            float lineY = minY + y * cellSize;

            GameObject line = Instantiate(linePrefab, parent);
            line.name = "Horizontal_Line_" + y;
            line.transform.position = new Vector3(centerX, lineY, 0f);
            line.transform.localScale = new Vector3(width, lineThickness, 1f);

            ApplyColor(line, gridColor);
            spawnedLines.Add(line);
        }
    }

    public void Clear()
    {
        foreach (GameObject line in spawnedLines)
        {
            if (line != null)
            {
                Destroy(line);
            }
        }

        spawnedLines.Clear();
    }

    private void ApplyColor(GameObject line, Color color)
    {
        SpriteRenderer spriteRenderer = line.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = -40;
        }
    }
}