using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelUI : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioManager audioManager;

    [Header("Car Speed")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TMP_Text speedValueText;
    [SerializeField] private CarController carController;

    private void Start()
    {
        SetupAudio();
        SetupSpeed();
    }

    private void OnDestroy()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        if (speedSlider != null)
            speedSlider.onValueChanged.RemoveListener(OnSpeedChanged);
    }

    private void SetupAudio()
    {
        if (audioManager == null)
            return;

        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.SetValueWithoutNotify(audioManager.SfxVolume);
            sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        }
    }

    private void SetupSpeed()
    {
        if (carController == null || speedSlider == null)
            return;

        speedSlider.minValue = carController.GetMinSpeed();
        speedSlider.maxValue = carController.GetMaxSpeed();
        speedSlider.wholeNumbers = false;
        speedSlider.SetValueWithoutNotify(carController.GetMoveSpeed());
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        RefreshSpeedText(carController.GetMoveSpeed());
    }

    private void OnMusicChanged(float value)
    {
        audioManager?.SetMusicVolume(value);
    }

    private void OnSfxChanged(float value)
    {
        audioManager?.SetSfxVolume(value);
    }

    private void OnSpeedChanged(float value)
    {
        if (carController == null)
            return;

        carController.SetMoveSpeed(value);
        RefreshSpeedText(value);
    }

    private void RefreshSpeedText(float value)
    {
        if (speedValueText != null)
            speedValueText.text = value.ToString("0.0");
    }
}