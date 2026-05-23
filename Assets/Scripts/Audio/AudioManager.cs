using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowClickClip;
    [SerializeField] private AudioClip missClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

    [Header("Volume")]
    [SerializeField] private float musicVolume = 0.4f;

    [SerializeField] private float arrowClickVolume = 0.25f;
    [SerializeField] private float missVolume = 0.35f;
    [SerializeField] private float winVolume = 1.0f;
    [SerializeField] private float loseVolume = 1.0f;

    private bool musicWasStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateAudioSourcesIfNeeded();
        SetupAudio();
    }

    private void Start()
    {
        ApplySettings();
    }

    private void CreateAudioSourcesIfNeeded()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void SetupAudio()
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = musicVolume;

        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = 1f;
    }

    public void ApplySettings()
    {
        bool musicEnabled = SettingsManager.IsMusicEnabled();
        bool soundsEnabled = SettingsManager.IsSoundsEnabled();

        musicSource.mute = !musicEnabled;
        sfxSource.mute = !soundsEnabled;

        if (musicEnabled)
        {
            PlayMusic();
        }
        else
        {
            PauseMusic();
        }
    }

    public void PlayMusic()
    {
        if (backgroundMusic == null)
            return;

        if (!SettingsManager.IsMusicEnabled())
            return;

        if (musicSource.isPlaying)
            return;

        if (musicWasStarted)
        {
            musicSource.UnPause();
        }
        else
        {
            musicSource.Play();
            musicWasStarted = true;
        }
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicWasStarted = false;
    }

    public void PlayArrowClick()
    {
        PlaySfx(arrowClickClip, arrowClickVolume);
    }

    public void PlayMiss()
    {
        PlaySfx(missClip, missVolume);
    }

    public void PlayWin()
    {
        PlaySfx(winClip, winVolume);
    }

    public void PlayLose()
    {
        PlaySfx(loseClip, loseVolume);
    }

    private void PlaySfx(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        if (!SettingsManager.IsSoundsEnabled())
            return;

        sfxSource.PlayOneShot(clip, volume);
    }
}