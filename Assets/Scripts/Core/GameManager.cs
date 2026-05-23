using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Lives { get; private set; }
    public GameState CurrentState { get; private set; }

    public bool IsPlaying => CurrentState == GameState.Playing;

    public void StartLevel(int startLives)
    {
        Lives = startLives;
        CurrentState = GameState.Playing;

        Debug.Log("Game started. Lives: " + Lives);
    }

    public void RegisterMistake()
    {
        if (!IsPlaying)
            return;

        Lives--;

        Debug.Log("Mistake. Lives left: " + Lives);

        if (Lives <= 0)
        {
            LoseLevel();
        }
    }

    public void CompleteLevel()
    {
        if (!IsPlaying)
            return;

        CurrentState = GameState.Win;
        Debug.Log("Level completed!");
    }

    private void LoseLevel()
    {
        CurrentState = GameState.Lose;
        Debug.Log("Game Over!");
    }
}