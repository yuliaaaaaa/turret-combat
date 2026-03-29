using UnityEngine;
using Zenject;

public class LevelProgressUpdater : MonoBehaviour
{
    [SerializeField] private Transform carTransform;

    private LevelProgressService _progressService;
    private GameStateService _gameStateService;

    [Inject]
    public void Construct(LevelProgressService progressService, GameStateService gameStateService)
    {
        _progressService = progressService;
        _gameStateService = gameStateService;
    }

    private void Update()
    {
        if (carTransform == null || _progressService == null || _gameStateService == null)
            return;

        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        _progressService.UpdateProgress(carTransform.position.z);
    }
}