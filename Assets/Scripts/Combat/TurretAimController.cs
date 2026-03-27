using UnityEngine;
using Zenject;

public class TurretAimController : MonoBehaviour
{
    [SerializeField] private PlayerInputReader inputReader;
    [SerializeField] private Transform turretPivot;

    [Header("Aim Settings")]
    [SerializeField] private float rotationSpeed = 120f;
    [SerializeField] private float minYaw = -70f;
    [SerializeField] private float maxYaw = 70f;
    [SerializeField] private float smoothing = 12f;

    private GameManager _gameManager;

    private float _targetYaw;
    private float _currentYaw;

    [Inject]
    public void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        ResetTurret();
    }

    private void Update()
    {
        if (_gameManager == null)
            return;

        if (_gameManager.CurrentState != GameState.Playing)
        {
            ResetTurretSmooth();
            return;
        }

        float input = inputReader.AimInput;

        _targetYaw += input * rotationSpeed * Time.deltaTime;
        _targetYaw = Mathf.Clamp(_targetYaw, minYaw, maxYaw);

        _currentYaw = Mathf.Lerp(_currentYaw, _targetYaw, smoothing * Time.deltaTime);
        ApplyRotation(_currentYaw);
    }

    private void ResetTurret()
    {
        _targetYaw = 0f;
        _currentYaw = 0f;
        ApplyRotation(0f);
    }

    private void ResetTurretSmooth()
    {
        _targetYaw = 0f;
        _currentYaw = Mathf.Lerp(_currentYaw, 0f, smoothing * Time.deltaTime);
        ApplyRotation(_currentYaw);
    }

    private void ApplyRotation(float yaw)
    {
        turretPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }
}