using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameUI gameUI;

    private int currentLevelId;
    private int arrowsLeft;
    private bool inputBlocked;

    public bool IsLevelPlaying => gameManager != null && gameManager.IsPlaying;

    public bool CanUseGameInput => IsLevelPlaying && !inputBlocked;

    public void InitializeLevel(int levelId, int arrowsCount)
    {
        currentLevelId = levelId;
        arrowsLeft = arrowsCount;
        inputBlocked = false;

        gameManager.StartLevel(3);

        gameUI.HideResults();
        gameUI.UpdateLives(gameManager.Lives);

        Debug.Log("Level initialized. Level ID: " + currentLevelId);
        Debug.Log("Arrows left: " + arrowsLeft);
    }

    public void SetInputBlocked(bool blocked)
    {
        inputBlocked = blocked;
    }

    public void OnArrowEscaped()
    {
        if (!CanUseGameInput)
            return;

        arrowsLeft--;

        Debug.Log("Arrow escaped. Arrows left: " + arrowsLeft);

        if (arrowsLeft <= 0)
        {
            inputBlocked = true;

            gameManager.CompleteLevel();

            SaveManager.MarkLevelAsCompleted(currentLevelId);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayWin();
            }

            gameUI.ShowWin();
        }
    }

    public void OnArrowCollision()
    {
        if (!CanUseGameInput)
            return;

        gameManager.RegisterMistake();
        gameUI.UpdateLives(gameManager.Lives);

        if (gameManager.CurrentState == GameState.Lose)
        {
            inputBlocked = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayLose();
            }

            gameUI.ShowLose();
        }
        else
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMiss();
            }
        }
    }
}