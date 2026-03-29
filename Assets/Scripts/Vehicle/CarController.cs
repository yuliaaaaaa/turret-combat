using UnityEngine;
using Zenject;

public class CarController : MonoBehaviour
{
    private const string SpeedKey = "CarSpeed";

    [Header("Speed")]
    [SerializeField] private float defaultMoveSpeed = 10f;
    [SerializeField] private float minMoveSpeed = 5f;
    [SerializeField] private float maxMoveSpeed = 25f;

    [Header("Lane Movement")]
    [SerializeField] private float xSmoothTime = 0.15f;

    [Header("Hit Reaction")]
    [SerializeField] private float hitOffsetDistance = 0.35f;
    [SerializeField] private float hitOffsetReturnSpeed = 3.5f;

    private GameStateService _gameStateService;

    private float _moveSpeed;
    private float _targetX;
    private float _xVelocity;
    private float _fixedY;
    private float _hitOffsetX;
    private bool _canMove = true;

    [Inject]
    public void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }

    private void Awake()
    {
        _fixedY = transform.position.y;
        LoadSpeed();
    }

    private void Update()
    {
        UpdateHitOffset();

        if (!CanMove())
            return;

        Vector3 position = transform.position;
        position.z += _moveSpeed * Time.deltaTime;
        position.x = Mathf.SmoothDamp(position.x, _targetX + _hitOffsetX, ref _xVelocity, xSmoothTime);
        position.y = _fixedY;

        transform.position = position;
    }

    public void SetTargetX(float value)
    {
        _targetX = value;
    }

    public void StopMovement()
    {
        _canMove = false;
    }

    public void ResumeMovement()
    {
        _canMove = true;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public float GetMinSpeed()
    {
        return minMoveSpeed;
    }

    public float GetMaxSpeed()
    {
        return maxMoveSpeed;
    }

    public void SetMoveSpeed(float value)
    {
        _moveSpeed = Mathf.Clamp(value, minMoveSpeed, maxMoveSpeed);
        PlayerPrefs.SetFloat(SpeedKey, _moveSpeed);
    }

    public void SaveSpeed()
    {
        PlayerPrefs.Save();
    }

    public void ApplyEnemyHit(float enemyWorldX)
    {
        float direction = enemyWorldX < transform.position.x ? 1f : -1f;
        _hitOffsetX = direction * hitOffsetDistance;
    }

    private void UpdateHitOffset()
    {
        _hitOffsetX = Mathf.MoveTowards(_hitOffsetX, 0f, hitOffsetReturnSpeed * Time.deltaTime);
    }

    private bool CanMove()
    {
        return _canMove &&
               _gameStateService != null &&
               _gameStateService.CurrentState == GameState.Playing;
    }

    private void LoadSpeed()
    {
        _moveSpeed = PlayerPrefs.GetFloat(SpeedKey, defaultMoveSpeed);
        _moveSpeed = Mathf.Clamp(_moveSpeed, minMoveSpeed, maxMoveSpeed);
    }
}