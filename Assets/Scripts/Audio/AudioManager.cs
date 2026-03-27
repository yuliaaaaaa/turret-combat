using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SfxVolume";

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip enemyHitClip;

    public float MusicVolume { get; private set; } = 1f;
    public float SfxVolume { get; private set; } = 1f;

    private void Awake()
    {
        LoadSettings();
        ApplyVolumes();

        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;

            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    public void PlayShot()
    {
        PlaySfx(shotClip);
    }

    public void PlayEnemyHit()
    {
        PlaySfx(enemyHitClip);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip, SfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(MusicKey, MusicVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    public void SetSfxVolume(float value)
    {
        SfxVolume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(SfxKey, SfxVolume);
        PlayerPrefs.Save();
        ApplyVolumes();
    }

    private void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat(MusicKey, 1f);
        SfxVolume = PlayerPrefs.GetFloat(SfxKey, 1f);
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
            musicSource.volume = MusicVolume;

        if (sfxSource != null)
            sfxSource.volume = SfxVolume;
    }
}