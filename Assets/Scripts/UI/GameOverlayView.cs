using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverlayView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text stateOverlayText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button fullscreenStateButton;

    public event Action FullscreenClicked;

    private void Awake()
    {
        if (fullscreenStateButton != null)
            fullscreenStateButton.onClick.AddListener(OnFullscreenClicked);
    }

    private void OnDestroy()
    {
        if (fullscreenStateButton != null)
            fullscreenStateButton.onClick.RemoveListener(OnFullscreenClicked);
    }

    public void ShowWaiting()
    {
        SetRootVisible(true);
        SetStateText("Tap to Start");
        SetResult(string.Empty, false);
    }

    public void ShowFinished(bool isWin)
    {
        SetRootVisible(true);
        SetStateText("Tap to Restart");
        SetResult(isWin ? "You Win" : "You Lose", true);
    }

    public void Hide()
    {
        SetRootVisible(false);
    }

    private void SetRootVisible(bool visible)
    {
        if (root != null)
            root.SetActive(visible);
    }

    private void SetStateText(string value)
    {
        if (stateOverlayText != null)
            stateOverlayText.text = value;
    }

    private void SetResult(string value, bool visible)
    {
        if (resultText == null)
            return;

        resultText.text = value;
        resultText.gameObject.SetActive(visible);
    }

    private void OnFullscreenClicked()
    {
        FullscreenClicked?.Invoke();
    }
}