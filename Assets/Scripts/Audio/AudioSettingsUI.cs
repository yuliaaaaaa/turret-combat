using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioManager audioManager;

    private void Start()
    {
        if (audioManager == null)
            return;

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
            musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.SetValueWithoutNotify(audioManager.SfxVolume);
            sfxSlider.onValueChanged.AddListener(audioManager.SetSfxVolume);
        }
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveAllListeners();

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveAllListeners();
    }
}