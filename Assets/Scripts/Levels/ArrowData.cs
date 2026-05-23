using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ArrowData
{
    public string arrowId;
    public ArrowDirection direction;
    public List<Vector2Int> cells;
}