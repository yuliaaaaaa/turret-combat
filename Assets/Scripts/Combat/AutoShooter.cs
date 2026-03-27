using UnityEngine;
using Zenject;

public class AutoShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shotsPerSecond = 5f;

    private EnemyEncounterTracker _enemyEncounterTracker;
    private GameManager _gameManager;

    private float _shotTimer;
    private bool _isShootingEnabled;

    [Inject]
    public void Construct(EnemyEncounterTracker enemyEncounterTracker, GameManager gameManager)
    {
        _enemyEncounterTracker = enemyEncounterTracker;
        _gameManager = gameManager;
    }

    private void OnEnable()
    {
        if (_enemyEncounterTracker != null)
        {
            _enemyEncounterTracker.EncounterStarted += OnEncounterStarted;
            _enemyEncounterTracker.EncounterEnded += OnEncounterEnded;

            _isShootingEnabled = _enemyEncounterTracker.HasActiveEnemies;
        }
    }

    private void OnDisable()
    {
        if (_enemyEncounterTracker != null)
        {
            _enemyEncounterTracker.EncounterStarted -= OnEncounterStarted;
            _enemyEncounterTracker.EncounterEnded -= OnEncounterEnded;
        }
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

        if (_gameManager == null || _gameManager.CurrentState != GameState.Playing)
            return false;

        if (firePoint == null || projectilePrefab == null)
            return false;

        return true;
    }

    private void Fire()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
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