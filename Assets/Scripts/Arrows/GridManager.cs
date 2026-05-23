using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Vector2Int gridSize;
    private Dictionary<Vector2Int, string> occupiedCells = new Dictionary<Vector2Int, string>();

    public void SetGridSize(Vector2Int size)
    {
        gridSize = size;
        Debug.Log("Grid size set: " + gridSize);
    }

    public void RegisterCell(Vector2Int cell, string arrowId)
    {
        if (occupiedCells.ContainsKey(cell))
        {
            Debug.LogWarning("Cell " + cell + " is already occupied by " + occupiedCells[cell]);
            return;
        }

        occupiedCells.Add(cell, arrowId);
        Debug.Log("Cell " + cell + " occupied by " + arrowId);
    }

    public void FreeCell(Vector2Int cell)
    {
        if (occupiedCells.ContainsKey(cell))
        {
            occupiedCells.Remove(cell);
        }
    }

    public bool IsOccupiedByOtherArrow(Vector2Int cell, string currentArrowId)
    {
        if (!occupiedCells.ContainsKey(cell))
        {
            return false;
        }

        return occupiedCells[cell] != currentArrowId;
    }

    public bool IsOutsideGrid(Vector2Int cell)
    {
        return cell.x < 0 ||
               cell.y < 0 ||
               cell.x >= gridSize.x ||
               cell.y >= gridSize.y;
    }

    public void ClearGrid()
    {
        occupiedCells.Clear();
        Debug.Log("Grid cleared");
    }
}