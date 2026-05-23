using UnityEngine;

public static class SaveManager
{
    private const string CompletedLevelKey = "Completed_Level_";
    private const string SelectedLevelKey = "Selected_Level_Id";

    public static void MarkLevelAsCompleted(int levelId)
    {
        PlayerPrefs.SetInt(CompletedLevelKey + levelId, 1);
        PlayerPrefs.Save();

        Debug.Log("Level saved as completed: " + levelId);
    }

    public static bool IsLevelCompleted(int levelId)
    {
        return PlayerPrefs.GetInt(CompletedLevelKey + levelId, 0) == 1;
    }

    public static void SetSelectedLevelId(int levelId)
    {
        PlayerPrefs.SetInt(SelectedLevelKey, levelId);
        PlayerPrefs.Save();

        Debug.Log("Selected level: " + levelId);
    }

    public static int GetSelectedLevelId()
    {
        return PlayerPrefs.GetInt(SelectedLevelKey, 1);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Progress reset");
    }

    public static void ResetCompletedLevels()
    {
        PlayerPrefs.DeleteKey(CompletedLevelKey + 1);
        PlayerPrefs.DeleteKey(CompletedLevelKey + 2);
        PlayerPrefs.DeleteKey(CompletedLevelKey + 3);

        PlayerPrefs.Save();

        Debug.Log("Completed levels reset");
    }
}