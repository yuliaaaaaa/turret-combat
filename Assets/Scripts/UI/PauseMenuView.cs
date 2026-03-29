using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;

    public event Action ResumeClicked;
    public event Action RestartClicked;

    private void Awake()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnDestroy()
    {
        if (resumeButton != null)
            resumeButton.onClick.RemoveListener(OnResumeClicked);

        if (restartButton != null)
            restartButton.onClick.RemoveListener(OnRestartClicked);
    }

    public void SetVisible(bool visible)
    {
        if (root != null)
            root.SetActive(visible);
    }

    private void OnResumeClicked()
    {
        ResumeClicked?.Invoke();
    }

    private void OnRestartClicked()
    {
        RestartClicked?.Invoke();
    }
}