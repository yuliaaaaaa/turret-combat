using System;
using System.Collections;
using UnityEngine;
using Zenject;

public enum EnemyType
{
    Runner = 0,
    Ambush = 1
}

[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("References")]
    [SerializeField] protected EnemyAnimator enemyAnimator;
    [SerializeField] private Collider[] collidersToDisable;

    protected Health Health { get; private set; }
    protected Transform VehicleTransform { get; private set; }
    protected VehicleDamageReceiver VehicleHealth { get; private set; }
    protected EnemyConfig Config { get; private set; }
    protected GameManager GameManager { get; private set; }

    protected bool IsDead { get; private set; }
    protected bool HasAttacked { get; private set; }
    protected bool IsFinishing { get; private set; }

    private Coroutine _lifecycleRoutine;

    public bool CanBeForceDespawned => !IsFinishing;

    public event Action<Enemy> Removed;

    [Inject]
    public void Construct(VehicleDamageReceiver vehicleHealth, GameManager gameManager)
    {
        VehicleTransform = vehicleHealth != null ? vehicleHealth.transform : null;
        VehicleHealth = vehicleHealth;
        GameManager = gameManager;
    }

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();

        if (Health != null)
            Health.Died += OnDied;

        if (collidersToDisable == null || collidersToDisable.Length == 0)
            collidersToDisable = GetComponentsInChildren<Collider>(true);
    }

    protected virtual void OnDestroy()
    {
        if (Health != null)
            Health.Died -= OnDied;

        Removed?.Invoke(this);
    }

    public virtual void Initialize(EnemyConfig config)
    {
        Config = config;
        IsDead = false;
        HasAttacked = false;
        IsFinishing = false;

        if (Health != null && Config != null)
            Health.Initialize(Config.MaxHealth);

        SetCollidersEnabled(true);
        enemyAnimator?.ResetToIdle();
    }

    protected virtual void Update()
    {
        if (!CanUpdateBehaviour())
            return;

        float sqrDistance = GetSqrDistanceToVehicle();
        Tick(Mathf.Sqrt(sqrDistance));
    }

    protected abstract void Tick(float distanceToVehicle);

    protected void SetAnimationState(EnemyVisualState state)
    {
        enemyAnimator?.SetState(state);
    }

    protected void MoveTowardsVehicle(float speed, float rotationSpeed = 10f)
    {
        if (VehicleTransform == null)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = VehicleTransform.position;
        targetPosition.y = currentPosition.y;

        Vector3 direction = targetPosition - currentPosition;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
    }

    protected void FaceVehicleInstant()
    {
        if (VehicleTransform == null)
            return;

        Vector3 direction = VehicleTransform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.0001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }

    public void TriggerAttack()
    {
        if (IsDead || HasAttacked || IsFinishing || Config == null)
            return;

        HasAttacked = true;
        IsFinishing = true;

        RestartLifecycleRoutine(AttackSequence());
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || Health == null)
            return;

        Health.TakeDamage(damage);

        if (!IsDead)
            enemyAnimator?.PlayHit();
    }

    public void ForceDespawnImmediate()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDied()
    {
        if (IsDead)
            return;

        IsDead = true;
        IsFinishing = true;

        SetCollidersEnabled(false);
        RestartLifecycleRoutine(DeathSequence());
    }

    private bool CanUpdateBehaviour()
    {
        if (IsDead || IsFinishing || Config == null || GameManager == null)
            return false;

        return GameManager.CurrentState == GameState.Playing;
    }

    private float GetSqrDistanceToVehicle()
    {
        if (VehicleTransform == null)
            return float.PositiveInfinity;

        Vector3 from = transform.position;
        Vector3 to = VehicleTransform.position;
        from.y = 0f;
        to.y = 0f;

        return (to - from).sqrMagnitude;
    }

    private IEnumerator AttackSequence()
    {
        SetCollidersEnabled(false);
        FaceVehicleInstant();
        enemyAnimator?.PlayAttack();

        yield return new WaitForSeconds(Config.AttackDelay);

        if (!IsDead)
            VehicleHealth?.ApplyEnemyDamage(Config.Damage, transform.position.x);

        if (!IsDead)
            enemyAnimator?.PlayFall();

        yield return new WaitForSeconds(Config.DespawnDelayAfterFall);

        Destroy(gameObject);
    }

    private IEnumerator DeathSequence()
    {
        SetCollidersEnabled(false);
        enemyAnimator?.PlayFall();

        float despawnDelay = Config != null ? Config.DespawnDelayAfterFall : 1.5f;
        yield return new WaitForSeconds(despawnDelay);

        Destroy(gameObject);
    }

    private void RestartLifecycleRoutine(IEnumerator routine)
    {
        if (_lifecycleRoutine != null)
            StopCoroutine(_lifecycleRoutine);

        _lifecycleRoutine = StartCoroutine(routine);
    }

    private void SetCollidersEnabled(bool enabledState)
    {
        if (collidersToDisable == null)
            return;

        for (int i = 0; i < collidersToDisable.Length; i++)
        {
            if (collidersToDisable[i] != null)
                collidersToDisable[i].enabled = enabledState;
        }
    }
}