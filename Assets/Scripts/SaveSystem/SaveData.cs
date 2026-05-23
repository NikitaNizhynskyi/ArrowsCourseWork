using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public List<int> completedLevels = new List<int>();

    public bool isFullscreen;
    public bool soundEnabled;
    public bool musicEnabled;
}