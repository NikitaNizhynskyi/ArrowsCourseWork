using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle soundsToggle;

    private void Start()
    {
        LoadSettingsToUI();

        fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        musicToggle.onValueChanged.AddListener(OnMusicChanged);
        soundsToggle.onValueChanged.AddListener(OnSoundsChanged);
    }

    private void LoadSettingsToUI()
    {
        fullscreenToggle.SetIsOnWithoutNotify(SettingsManager.IsFullscreen());
        musicToggle.SetIsOnWithoutNotify(SettingsManager.IsMusicEnabled());
        soundsToggle.SetIsOnWithoutNotify(SettingsManager.IsSoundsEnabled());

        SettingsManager.ApplySettings();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplySettings();
        }
    }

    private void OnFullscreenChanged(bool value)
    {
        SettingsManager.SetFullscreen(value);
    }

    private void OnMusicChanged(bool value)
    {
        SettingsManager.SetMusicEnabled(value);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplySettings();
        }
    }

    private void OnSoundsChanged(bool value)
    {
        SettingsManager.SetSoundsEnabled(value);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplySettings();
        }
    }
}