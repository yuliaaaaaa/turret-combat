using UnityEngine;
using Zenject;

public class AutoShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shotsPerSecond = 5f;

    private EnemyEncounterService _enemyEncounterService;
    private GameStateService _gameStateService;
    private DiContainer _container;

    private float _shotTimer;
    private bool _isShootingEnabled;

    [Inject]
    public void Construct(
        EnemyEncounterService enemyEncounterService,
        GameStateService gameStateService,
        DiContainer container)
    {
        _enemyEncounterService = enemyEncounterService;
        _gameStateService = gameStateService;
        _container = container;
    }

    private void OnEnable()
    {
        if (_enemyEncounterService == null)
            return;

        _enemyEncounterService.EncounterStarted += OnEncounterStarted;
        _enemyEncounterService.EncounterEnded += OnEncounterEnded;

        _isShootingEnabled = _enemyEncounterService.HasActiveEnemies;
    }

    private void OnDisable()
    {
        if (_enemyEncounterService == null)
            return;

        _enemyEncounterService.EncounterStarted -= OnEncounterStarted;
        _enemyEncounterService.EncounterEnded -= OnEncounterEnded;
    }

    private void Update()
    {
        if (!CanShoot())
            return;

        _shotTimer += Time.deltaTime;

        float shotInterval = 1f / shotsPerSecond;
        while (_shotTimer >= shotInterval)
        {
            _shotTimer -= shotInterval;
            Fire();
        }
    }

    private bool CanShoot()
    {
        if (!_isShootingEnabled)
            return false;

        if (_gameStateService == null || _gameStateService.CurrentState != GameState.Playing)
            return false;

        if (firePoint == null || projectilePrefab == null || _container == null)
            return false;

        return true;
    }

    private void Fire()
    {
        _container.InstantiatePrefabForComponent<Projectile>(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation,
            null);
    }

    private void OnEncounterStarted()
    {
        _isShootingEnabled = true;
        _shotTimer = 0f;
    }

    private void OnEncounterEnded()
    {
        _isShootingEnabled = false;
        _shotTimer = 0f;
    }
}