using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainButtonsGroup;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Start()
    {
        SettingsManager.ApplySettings();
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainButtonsGroup.SetActive(true);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    public void ShowSettings()
    {
        mainButtonsGroup.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }

    public void ShowCredits()
    {
        mainButtonsGroup.SetActive(false);

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }

    public void HideSettings()
    {
        ShowMainMenu();
    }

    public void HideCredits()
    {
        ShowMainMenu();
    }
}