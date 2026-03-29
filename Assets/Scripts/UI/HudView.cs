using System;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button settingsButton;

    public event Action PauseClicked;
    public event Action SettingsClicked;

    private void Awake()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnDestroy()
    {
        if (pauseButton != null)
            pauseButton.onClick.RemoveListener(OnPauseClicked);

        if (settingsButton != null)
            settingsButton.onClick.RemoveListener(OnSettingsClicked);
    }

    public void SetRootVisible(bool visible)
    {
        if (root != null)
            root.SetActive(visible);
    }

    public void SetPauseVisible(bool visible)
    {
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(visible);
    }

    public void SetSettingsVisible(bool visible)
    {
        if (settingsButton != null)
            settingsButton.gameObject.SetActive(visible);
    }

    private void OnPauseClicked()
    {
        PauseClicked?.Invoke();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
    }
}