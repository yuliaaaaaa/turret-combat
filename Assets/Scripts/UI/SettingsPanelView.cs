using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanelView : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button closeButton;

    [Header("Audio")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Car Speed")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TMP_Text speedValueText;

    private AudioManager _audioManager;
    private CarController _carController;

    public bool IsOpen => root != null && root.activeSelf;

    public event Action CloseClicked;

    [Inject]
    public void Construct(AudioManager audioManager, CarController carController)
    {
        _audioManager = audioManager;
        _carController = carController;
    }

    private void Awake()
    {
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
    }

    private void Start()
    {
        SetupAudio();
        SetupSpeed();
        Hide();
    }

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(OnCloseClicked);

        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);

        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);

        if (speedSlider != null)
            speedSlider.onValueChanged.RemoveListener(OnSpeedChanged);
    }

    public void Show()
    {
        if (root != null)
            root.SetActive(true);
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }

    private void SetupAudio()
    {
        if (_audioManager == null)
            return;

        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.SetValueWithoutNotify(_audioManager.MusicVolume);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.minValue = 0f;
            sfxSlider.maxValue = 1f;
            sfxSlider.SetValueWithoutNotify(_audioManager.SfxVolume);
            sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        }
    }

    private void SetupSpeed()
    {
        if (_carController == null || speedSlider == null)
            return;

        speedSlider.minValue = _carController.GetMinSpeed();
        speedSlider.maxValue = _carController.GetMaxSpeed();
        speedSlider.wholeNumbers = false;
        speedSlider.SetValueWithoutNotify(_carController.GetMoveSpeed());
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        RefreshSpeedText(_carController.GetMoveSpeed());
    }

    private void OnMusicChanged(float value)
    {
        _audioManager?.SetMusicVolume(value);
    }

    private void OnSfxChanged(float value)
    {
        _audioManager?.SetSfxVolume(value);
    }

    private void OnSpeedChanged(float value)
    {
        if (_carController == null)
            return;

        _carController.SetMoveSpeed(value);
        RefreshSpeedText(value);
    }

    private void RefreshSpeedText(float value)
    {
        if (speedValueText != null)
            speedValueText.text = value.ToString("0.0");
    }

    private void OnCloseClicked()
    {
        CloseClicked?.Invoke();
    }
}