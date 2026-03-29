using UnityEngine;
using Zenject;

public class GameTimeController : MonoBehaviour
{
    private GameStateService _gameStateService;

    [Inject]
    public void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    private void OnEnable()
    {
        if (_gameStateService != null)
            _gameStateService.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        if (_gameStateService != null)
            _gameStateService.StateChanged -= OnStateChanged;
    }

    private void Start()
    {
        if (_gameStateService != null)
            ApplyTimeScale(_gameStateService.CurrentState);
    }

    private void OnStateChanged(GameState state)
    {
        ApplyTimeScale(state);
    }

    private void ApplyTimeScale(GameState state)
    {
        Time.timeScale = state == GameState.Paused ? 0f : 1f;
    }
}