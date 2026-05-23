using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmExitPopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject gameplayButtonsGroup;
    [SerializeField] private LevelController levelController;
    [SerializeField] private string sceneToLoad = "LevelSelectScene";

    public void Show()
    {
        panel.SetActive(true);

        if (gameplayButtonsGroup != null)
        {
            gameplayButtonsGroup.SetActive(false);
        }

        if (levelController != null)
        {
            levelController.SetInputBlocked(true);
        }

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        panel.SetActive(false);

        if (gameplayButtonsGroup != null)
        {
            gameplayButtonsGroup.SetActive(true);
        }

        if (levelController != null)
        {
            levelController.SetInputBlocked(false);
        }

        Time.timeScale = 1f;
    }

    public void ConfirmExit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }
}