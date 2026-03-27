using UnityEngine;
using Zenject;

public class TurretAimController : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private Transform turretPivot;

    [Header("Yaw Limits")]
    [SerializeField] private float minYaw = -70f;
    [SerializeField] private float maxYaw = 70f;

    [Header("Movement Feel")]
    [SerializeField] private float maxTurnSpeed = 140f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 14f;
    [SerializeField] private float rotationSmoothTime = 0.08f;

    private GameManager _gameManager;

    private float _targetYaw;
    private float _currentYaw;
    private float _currentTurnSpeed;
    private float _yawSmoothVelocity;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        ResetTurretImmediate();
    }

    private void Update()
    {
        if (_gameManager == null || turretPivot == null || inputReader == null)
            return;

        if (_gameManager.CurrentState != GameState.Playing)
        {
            ReturnToCenter();
            return;
        }

        UpdateAim();
    }

    private void UpdateAim()
    {
        float input = inputReader.AimInput;
        float deltaTime = Time.deltaTime;

        float desiredTurnSpeed = input * maxTurnSpeed;

        float speedLerp = Mathf.Abs(input) > 0.001f ? acceleration : deceleration;
        _currentTurnSpeed = Mathf.Lerp(_currentTurnSpeed, desiredTurnSpeed, speedLerp * deltaTime);

        _targetYaw += _currentTurnSpeed * deltaTime;
        _targetYaw = Mathf.Clamp(_targetYaw, minYaw, maxYaw);

        _currentYaw = Mathf.SmoothDampAngle(
            _currentYaw,
            _targetYaw,
            ref _yawSmoothVelocity,
            rotationSmoothTime
        );

        ApplyRotation(_currentYaw);
    }

    private void ReturnToCenter()
    {
        _currentTurnSpeed = Mathf.Lerp(_currentTurnSpeed, 0f, deceleration * Time.deltaTime);
        _targetYaw = Mathf.Lerp(_targetYaw, 0f, deceleration * Time.deltaTime);

        _currentYaw = Mathf.SmoothDampAngle(
            _currentYaw,
            _targetYaw,
            ref _yawSmoothVelocity,
            rotationSmoothTime
        );

        ApplyRotation(_currentYaw);
    }

    private void ResetTurretImmediate()
    {
        _targetYaw = 0f;
        _currentYaw = 0f;
        _currentTurnSpeed = 0f;
        _yawSmoothVelocity = 0f;

        ApplyRotation(0f);
    }

    private void ApplyRotation(float yaw)
    {
        turretPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }
}