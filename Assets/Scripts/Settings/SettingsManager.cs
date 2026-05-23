using UnityEngine;

public static class SettingsManager
{
    private const string FullscreenKey = "Setting_Fullscreen";
    private const string MusicKey = "Setting_Music";
    private const string SoundsKey = "Setting_Sounds";

    public static bool IsFullscreen()
    {
        return PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
    }

    public static bool IsMusicEnabled()
    {
        return PlayerPrefs.GetInt(MusicKey, 1) == 1;
    }

    public static bool IsSoundsEnabled()
    {
        return PlayerPrefs.GetInt(SoundsKey, 1) == 1;
    }

    public static void SetFullscreen(bool value)
    {
        PlayerPrefs.SetInt(FullscreenKey, value ? 1 : 0);
        PlayerPrefs.Save();

        ApplyFullscreen(value);

        Debug.Log("Fullscreen: " + value);
    }

    public static void SetMusicEnabled(bool value)
    {
        PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Music: " + value);
    }

    public static void SetSoundsEnabled(bool value)
    {
        PlayerPrefs.SetInt(SoundsKey, value ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Sounds: " + value);
    }

    public static void ApplySettings()
    {
        ApplyFullscreen(IsFullscreen());
    }

    private static void ApplyFullscreen(bool enabled)
    {
        if (enabled)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
        }
    }
}