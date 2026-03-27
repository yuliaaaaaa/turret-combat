using UnityEngine;
using Zenject;

[RequireComponent(typeof(Health))]
public class VehicleHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    [Header("Hit VFX")]
    [SerializeField] private GameObject hitVFXPrefab;
    [SerializeField] private float hitVFXLifetime = 2f;
    [SerializeField] private Collider vehicleHitCollider;
    [SerializeField] private Vector3 hitVFXOffset = new Vector3(0f, 0.5f, 0f);

    public Health Health { get; private set; }

    private CarController _carController;

    [Inject]
    public void Construct(CarController carController)
    {
        _carController = carController;
    }

    private void Awake()
    {
        Health = GetComponent<Health>();

        if (vehicleHitCollider == null)
            vehicleHitCollider = GetComponentInChildren<Collider>();
    }

    private void Start()
    {
        if (Health != null)
            Health.Initialize(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        Health?.TakeDamage(damage);
    }

    public void TakeDamageFromEnemy(int damage, float enemyWorldX)
    {
        Health?.TakeDamage(damage);
        _carController?.ApplyEnemyHit(enemyWorldX);
        SpawnHitVFX(enemyWorldX);
    }

    private void SpawnHitVFX(float enemyWorldX)
    {
        if (hitVFXPrefab == null)
            return;

        Vector3 spawnPosition;

        if (vehicleHitCollider != null)
        {
            Vector3 enemyPoint = new Vector3(enemyWorldX, vehicleHitCollider.bounds.center.y, vehicleHitCollider.bounds.center.z);
            spawnPosition = vehicleHitCollider.ClosestPoint(enemyPoint);
        }
        else
        {
            float direction = enemyWorldX < transform.position.x ? -1f : 1f;
            spawnPosition = transform.position + new Vector3(direction * 0.8f, 0.5f, 0f);
        }

        spawnPosition += hitVFXOffset;

        GameObject vfx = Instantiate(hitVFXPrefab, spawnPosition, Quaternion.identity);
        Destroy(vfx, hitVFXLifetime);
    }
}