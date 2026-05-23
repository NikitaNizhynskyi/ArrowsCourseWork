using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelId;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private string gameSceneName = "GameScene";

    [Header("Status Colors")]
    [SerializeField] private Color completedColor = new Color32(46, 173, 74, 255);
    [SerializeField] private Color notCompletedColor = new Color32(0, 0, 0, 255);

    private void Start()
    {
        UpdateStatusText();
    }

    public void OpenLevel()
    {
        SaveManager.SetSelectedLevelId(levelId);
        SceneManager.LoadScene(gameSceneName);
    }

    private void UpdateStatusText()
    {
        if (statusText == null)
        {
            return;
        }

        if (SaveManager.IsLevelCompleted(levelId))
        {
            statusText.text = "Won!";
            statusText.color = completedColor;
        }
        else
        {
            statusText.text = "Not completed";
            statusText.color = notCompletedColor;
        }
    }
}