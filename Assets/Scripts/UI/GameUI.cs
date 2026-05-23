using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text livesText;

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;
    [SerializeField] private GameObject backToLevelsButton;

    [Header("Gameplay UI")]
    [SerializeField] private GameObject gameplayButtonsGroup;

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void ShowWin()
    {
        resultPanel.SetActive(true);

        winText.SetActive(true);
        loseText.SetActive(false);

        backToLevelsButton.SetActive(true);

        if (gameplayButtonsGroup != null)
        {
            gameplayButtonsGroup.SetActive(false);
        }
    }

    public void ShowLose()
    {
        resultPanel.SetActive(true);

        loseText.SetActive(true);
        winText.SetActive(false);

        backToLevelsButton.SetActive(true);

        if (gameplayButtonsGroup != null)
        {
            gameplayButtonsGroup.SetActive(false);
        }
    }

    public void HideResults()
    {
        resultPanel.SetActive(false);

        winText.SetActive(false);
        loseText.SetActive(false);
        backToLevelsButton.SetActive(false);

        if (gameplayButtonsGroup != null)
        {
            gameplayButtonsGroup.SetActive(true);
        }
    }
}