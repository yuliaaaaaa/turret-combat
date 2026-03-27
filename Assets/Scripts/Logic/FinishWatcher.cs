using UnityEngine;
using Zenject;

public class FinishWatcher : MonoBehaviour
{
    [SerializeField] private Transform carTransform;
    [SerializeField] private float finishDistance = 1f;

    private GameManager _gameManager;
    private RoadGenerator _roadGenerator;
    private Transform _finishPoint;
    private bool _isTriggered;

    [Inject]
    public void Construct(GameManager gameManager, RoadGenerator roadGenerator)
    {
        _gameManager = gameManager;
        _roadGenerator = roadGenerator;
    }

    private void Update()
    {
        if (_isTriggered || carTransform == null || _gameManager == null || _roadGenerator == null)
            return;

        if (_gameManager.CurrentState != GameState.Playing)
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
            _gameManager.ReachFinish();
        }
    }

    public void ResetWatcher()
    {
        _isTriggered = false;
        _finishPoint = null;
    }
}