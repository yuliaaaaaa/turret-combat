using UnityEngine;
using Zenject;

public class FinishWatcher : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float finishDistance = 1f;

    private GameStateService _gameStateService;
    private RoadGenerator _roadGenerator;
    private LevelFlowCoordinator _levelFlowCoordinator;

    private Transform _finishPoint;
    private bool _isTriggered;

    [Inject]
    public void Construct(
        GameStateService gameStateService,
        RoadGenerator roadGenerator,
        LevelFlowCoordinator levelFlowCoordinator)
    {
        _gameStateService = gameStateService;
        _roadGenerator = roadGenerator;
        _levelFlowCoordinator = levelFlowCoordinator;
    }

    private void Update()
    {
        if (_isTriggered || carTransform == null || _gameStateService == null || _roadGenerator == null)
            return;

        if (_gameStateService.CurrentState != GameState.Playing)
            return;

        if (_finishPoint == null)
        {
            _finishPoint = _roadGenerator.GetFinishPoint();

            if (_finishPoint == null)
                return;
        }

        float distance = Vector3.Distance(carTransform.position, _finishPoint.position);

        if (distance <= finishDistance)
        {
            _isTriggered = true;
            _levelFlowCoordinator?.ReachFinish();
        }
    }

    public void ResetWatcher()
    {
        _isTriggered = false;
        _finishPoint = null;
    }
}