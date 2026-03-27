using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private Button fullscreenStateButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text stateOverlayText;
    [SerializeField] private TMP_Text resultText;

    [Header("Panels")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject stateOverlayRoot;

    private GameManager _gameManager;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        BindButtons();
    }

    private void OnEnable()
    {
        if (_gameManager != null)
            _gameManager.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        if (_gameManager != null)
            _gameManager.StateChanged -= OnStateChanged;
    }

    private void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (_gameManager != null)
            OnStateChanged(_gameManager.CurrentState);
    }

    private void OnPauseClicked()
    {
        _gameManager?.PauseGame();
    }

    private void OnResumeClicked()
    {
        if (settingsPanel != null && settingsPanel.activeSelf)
            return;

        _gameManager?.ResumeGame();
    }

    private void OnRestartClicked()
    {
        _gameManager?.RestartGame();
    }

    private void OnOpenSettingsClicked()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        if (_gameManager != null && _gameManager.CurrentState == GameState.Playing)
            _gameManager.PauseGame();
    }

    private void OnCloseSettingsClicked()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (_gameManager != null && _gameManager.CurrentState == GameState.Paused)
            _gameManager.ResumeGame();
    }

    private void OnFullscreenStateClicked()
    {
        if (_gameManager == null)
            return;

        if (settingsPanel != null && settingsPanel.activeSelf)
            return;

        switch (_gameManager.CurrentState)
        {
            case GameState.WaitingForStart:
                _gameManager.StartGame();
                break;

            case GameState.Win:
            case GameState.Lose:
                _gameManager.RestartGame();
                break;
        }
    }

    private void OnStateChanged(GameState state)
    {
        bool isWaiting = state == GameState.WaitingForStart;
        bool isPlaying = state == GameState.Playing;
        bool isPaused = state == GameState.Paused;
        bool isFinished = state == GameState.Win || state == GameState.Lose;

        if (hudPanel != null)
            hudPanel.SetActive(true);

        if (pauseButton != null)
            pauseButton.gameObject.SetActive(isPlaying);

        if (restartButton != null)
            restartButton.gameObject.SetActive(isPaused);

        if (settingsButton != null)
            settingsButton.gameObject.SetActive(!isFinished);

        if (pausePanel != null)
        {
            bool shouldShowPausePanel = isPaused && (settingsPanel == null || !settingsPanel.activeSelf);
            pausePanel.SetActive(shouldShowPausePanel);
        }

        bool shouldShowStateOverlay = isWaiting || isFinished;

        if (stateOverlayRoot != null)
            stateOverlayRoot.SetActive(shouldShowStateOverlay);

        if (fullscreenStateButton != null)
            fullscreenStateButton.gameObject.SetActive(shouldShowStateOverlay);

        if (stateOverlayText != null)
        {
            stateOverlayText.text = state switch
            {
                GameState.WaitingForStart => "Tap to Start",
                GameState.Win => "Tap to Restart",
                GameState.Lose => "Tap to Restart",
                _ => string.Empty
            };
        }

        if (resultText != null)
        {
            resultText.text = state switch
            {
                GameState.Win => "You Win",
                GameState.Lose => "You Lose",
                _ => string.Empty
            };

            resultText.gameObject.SetActive(isFinished);
        }
    }

    private void BindButtons()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseClicked);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeClicked);

        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnOpenSettingsClicked);

        if (closeSettingsButton != null)
            closeSettingsButton.onClick.AddListener(OnCloseSettingsClicked);

        if (fullscreenStateButton != null)
            fullscreenStateButton.onClick.AddListener(OnFullscreenStateClicked);
    }
}