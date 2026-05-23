using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelId;
    public Vector2Int gridSize;
    public List<ArrowData> arrows;

    [Header("Visual Theme")]
    public Color backgroundColor = Color.white;
    public Color gridColor = Color.gray;
    public Color arrowSegmentColor = Color.black;
    public Color arrowHeadColor = Color.black;
}