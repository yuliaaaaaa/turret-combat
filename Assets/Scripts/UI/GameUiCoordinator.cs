using UnityEngine;
using Zenject;

public class GameUiCoordinator : MonoBehaviour
{
    private GameStateService _gameStateService;
    private LevelFlowCoordinator _levelFlowCoordinator;
    private HudView _hudView;
    private PauseMenuView _pauseMenuView;
    private GameOverlayView _overlayView;
    private SettingsPanelView _settingsPanelView;

    [Inject]
    public void Construct(
        GameStateService gameStateService,
        LevelFlowCoordinator levelFlowCoordinator,
        HudView hudView,
        PauseMenuView pauseMenuView,
        GameOverlayView overlayView,
        SettingsPanelView settingsPanelView)
    {
        _gameStateService = gameStateService;
        _levelFlowCoordinator = levelFlowCoordinator;
        _hudView = hudView;
        _pauseMenuView = pauseMenuView;
        _overlayView = overlayView;
        _settingsPanelView = settingsPanelView;
    }

    private void OnEnable()
    {
        SubscribeViews();

        if (_gameStateService != null)
            _gameStateService.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        UnsubscribeViews();

        if (_gameStateService != null)
            _gameStateService.StateChanged -= OnStateChanged;
    }

    private void Start()
    {
        if (_gameStateService != null)
            OnStateChanged(_gameStateService.CurrentState);
    }

    private void SubscribeViews()
    {
        if (_hudView != null)
        {
            _hudView.PauseClicked += OnPauseClicked;
            _hudView.SettingsClicked += OnSettingsClicked;
        }

        if (_pauseMenuView != null)
        {
            _pauseMenuView.ResumeClicked += OnResumeClicked;
            _pauseMenuView.RestartClicked += OnRestartClicked;
        }

        if (_overlayView != null)
            _overlayView.FullscreenClicked += OnOverlayClicked;

        if (_settingsPanelView != null)
            _settingsPanelView.CloseClicked += OnSettingsClosed;
    }

    private void UnsubscribeViews()
    {
        if (_hudView != null)
        {
            _hudView.PauseClicked -= OnPauseClicked;
            _hudView.SettingsClicked -= OnSettingsClicked;
        }

        if (_pauseMenuView != null)
        {
            _pauseMenuView.ResumeClicked -= OnResumeClicked;
            _pauseMenuView.RestartClicked -= OnRestartClicked;
        }

        if (_overlayView != null)
            _overlayView.FullscreenClicked -= OnOverlayClicked;

        if (_settingsPanelView != null)
            _settingsPanelView.CloseClicked -= OnSettingsClosed;
    }

    private void OnPauseClicked()
    {
        _levelFlowCoordinator?.PauseGame();
    }

    private void OnResumeClicked()
    {
        if (_gameStateService == null)
            return;

        if (_settingsPanelView != null && _settingsPanelView.IsOpen)
            return;

        _levelFlowCoordinator?.ResumeGame();
    }

    private void OnRestartClicked()
    {
        _levelFlowCoordinator?.RestartGame();
    }

    private void OnSettingsClicked()
    {
        _settingsPanelView?.Show();

        if (_gameStateService != null && _gameStateService.CurrentState == GameState.Playing)
            _levelFlowCoordinator?.PauseGame();

        RefreshPauseMenuVisibility();
    }

    private void OnSettingsClosed()
    {
        _settingsPanelView?.Hide();

        if (_gameStateService != null && _gameStateService.CurrentState == GameState.Paused)
            _levelFlowCoordinator?.ResumeGame();

        RefreshPauseMenuVisibility();
    }

    private void OnOverlayClicked()
    {
        if (_gameStateService == null)
            return;

        if (_settingsPanelView != null && _settingsPanelView.IsOpen)
            return;

        switch (_gameStateService.CurrentState)
        {
            case GameState.WaitingForStart:
                _levelFlowCoordinator?.StartGame();
                break;

            case GameState.Win:
            case GameState.Lose:
                _levelFlowCoordinator?.RestartGame(startImmediately: true);
                break;
        }
    }

    private void OnStateChanged(GameState state)
    {
        bool isWaiting = state == GameState.WaitingForStart;
        bool isPlaying = state == GameState.Playing;
        bool isFinished = state == GameState.Win || state == GameState.Lose;

        _hudView?.SetRootVisible(true);
        _hudView?.SetPauseVisible(isPlaying);
        _hudView?.SetSettingsVisible(!isFinished);

        RefreshPauseMenuVisibility();

        if (_overlayView == null)
            return;

        if (isWaiting)
            _overlayView.ShowWaiting();
        else if (state == GameState.Win)
            _overlayView.ShowFinished(true);
        else if (state == GameState.Lose)
            _overlayView.ShowFinished(false);
        else
            _overlayView.Hide();
    }

    private void RefreshPauseMenuVisibility()
    {
        if (_pauseMenuView == null || _gameStateService == null)
            return;

        bool shouldShowPauseMenu =
            _gameStateService.CurrentState == GameState.Paused &&
            (_settingsPanelView == null || !_settingsPanelView.IsOpen);

        _pauseMenuView.SetVisible(shouldShowPauseMenu);
    }
}