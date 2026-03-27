using UnityEngine;
using Zenject;

[RequireComponent(typeof(Health))]
public class VehicleDamageReceiver : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Hit VFX")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private Transform hitPoint;

    private Health _health;
    private CarController _carController;

    public Health Health => _health;

    [Inject]
    public void Construct(CarController carController)
    {
        _carController = carController;
    }

    private void Awake()
    {
        _health = GetComponent<Health>();

        if (_health == null)
        {
            Debug.LogError("CarDamageReceiver requires Health component.", this);
            return;
        }

        _health.Initialize(maxHealth);
    }

    public void ApplyEnemyDamage(int damage, float enemyWorldX)
    {
        if (_health == null)
            return;

        _health.TakeDamage(damage);

        _carController?.ApplyEnemyHit(enemyWorldX);

        SpawnHitVFX(enemyWorldX);
    }

    private void SpawnHitVFX(float enemyWorldX)
    {
        if (hitVFX == null)
            return;

        Vector3 spawnPosition;

        if (hitPoint != null)
        {
            spawnPosition = hitPoint.position;
        }
        else
        {
            float direction = enemyWorldX < transform.position.x ? -1f : 1f;
            spawnPosition = transform.position + new Vector3(direction * 0.5f, 1f, 0f);
        }

        Instantiate(hitVFX, spawnPosition, Quaternion.identity);
    }
}