using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public enum GameState
{
    WaitingForStart = 0,
    Playing = 1,
    Paused = 2,
    Win = 3,
    Lose = 4
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private VehicleHealth vehicleHealth;

    private CarController _carController;
    private CarFollowCamera _cameraFollow;
    private Health _vehicleRuntimeHealth;
    private Coroutine _finishRoutine;

    public GameState CurrentState { get; private set; } = GameState.WaitingForStart;
    public bool IsLevelFinishing { get; private set; }

    public event Action<GameState> StateChanged;

    [Inject]
    public void Construct(CarController carController, CarFollowCamera cameraFollow)
    {
        _carController = carController;
        _cameraFollow = cameraFollow;
    }

    private void Awake()
    {
        ApplyState(GameState.WaitingForStart, notify: false);
        IsLevelFinishing = false;
    }

    private void Start()
    {
        if (vehicleHealth == null)
            return;

        _vehicleRuntimeHealth = vehicleHealth.Health;
        if (_vehicleRuntimeHealth != null)
            _vehicleRuntimeHealth.Died += OnVehicleDied;

        StateChanged?.Invoke(CurrentState);
    }

    private void OnDestroy()
    {
        if (_vehicleRuntimeHealth != null)
            _vehicleRuntimeHealth.Died -= OnVehicleDied;
    }

    public void StartGame()
    {
        if (CurrentState != GameState.WaitingForStart)
            return;

        IsLevelFinishing = false;
        _carController?.ResumeMovement();
        ApplyState(GameState.Playing);
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        ApplyState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        ApplyState(GameState.Playing);
    }

    public void ReachFinish()
    {
        if (CurrentState != GameState.Playing || IsLevelFinishing)
            return;

        IsLevelFinishing = true;
        _carController?.StopMovement();

        if (_finishRoutine != null)
            StopCoroutine(_finishRoutine);

        _finishRoutine = StartCoroutine(FinishSequence());
    }

    public void LoseGame()
    {
        if (IsTerminalState(CurrentState))
            return;

        IsLevelFinishing = false;
        _carController?.StopMovement();

        if (_finishRoutine != null)
        {
            StopCoroutine(_finishRoutine);
            _finishRoutine = null;
        }

        ApplyState(GameState.Lose);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FinishSequence()
    {
        _cameraFollow?.PlayWinLook();

        yield return new WaitForSeconds(1.5f);

        if (!IsTerminalState(CurrentState))
            ApplyState(GameState.Win);

        _finishRoutine = null;
    }

    private void OnVehicleDied()
    {
        LoseGame();
    }

    private void ApplyState(GameState newState, bool notify = true)
    {
        CurrentState = newState;
        Time.timeScale = newState == GameState.Paused ? 0f : 1f;

        if (notify)
            StateChanged?.Invoke(CurrentState);
    }

    private static bool IsTerminalState(GameState state)
    {
        return state == GameState.Win || state == GameState.Lose;
    }
}