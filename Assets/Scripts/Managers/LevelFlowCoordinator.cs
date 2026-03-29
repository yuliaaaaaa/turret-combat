using System.Collections;
using UnityEngine;
using Zenject;

public class LevelFlowCoordinator : MonoBehaviour
{
    private GameStateService _gameStateService;
    private SceneReloadService _sceneReloadService;
    private CarController _carController;
    private CarFollowCamera _carFollowCamera;
    private VehicleDamageReceiver _vehicleHealth;

    private Health _vehicleRuntimeHealth;
    private Coroutine _finishRoutine;

    public bool IsLevelFinishing { get; private set; }

    [Inject]
    public void Construct(
        GameStateService gameStateService,
        SceneReloadService sceneReloadService,
        CarController carController,
        CarFollowCamera carFollowCamera,
        VehicleDamageReceiver vehicleHealth)
    {
        _gameStateService = gameStateService;
        _sceneReloadService = sceneReloadService;
        _carController = carController;
        _carFollowCamera = carFollowCamera;
        _vehicleHealth = vehicleHealth;
    }

    private void Awake()
    {
        IsLevelFinishing = false;
    }

    private void Start()
    {
        if (_vehicleHealth != null)
        {
            _vehicleRuntimeHealth = _vehicleHealth.Health;

            if (_vehicleRuntimeHealth != null)
                _vehicleRuntimeHealth.Died += OnVehicleDied;
        }

        if (_sceneReloadService != null && _sceneReloadService.ConsumeStartImmediatelyFlag())
            StartGame();
    }

    private void OnDestroy()
    {
        if (_vehicleRuntimeHealth != null)
            _vehicleRuntimeHealth.Died -= OnVehicleDied;
    }

    public void StartGame()
    {
        if (_gameStateService == null)
            return;

        if (_gameStateService.CurrentState != GameState.WaitingForStart)
            return;

        IsLevelFinishing = false;
        _carController?.ResumeMovement();
        _gameStateService.StartGame();
    }

    public void PauseGame()
    {
        _gameStateService?.PauseGame();
    }

    public void ResumeGame()
    {
        _gameStateService?.ResumeGame();
    }

    public void ReachFinish()
    {
        if (_gameStateService == null)
            return;

        if (_gameStateService.CurrentState != GameState.Playing || IsLevelFinishing)
            return;

        IsLevelFinishing = true;
        _carController?.StopMovement();

        if (_finishRoutine != null)
            StopCoroutine(_finishRoutine);

        _finishRoutine = StartCoroutine(FinishSequence());
    }

    public void RestartGame(bool startImmediately = false)
    {
        _sceneReloadService?.ReloadCurrentScene(startImmediately);
    }

    private IEnumerator FinishSequence()
    {
        _carFollowCamera?.PlayWinLook();

        yield return new WaitForSeconds(1.5f);

        if (_gameStateService != null &&
            _gameStateService.CurrentState != GameState.Win &&
            _gameStateService.CurrentState != GameState.Lose)
        {
            _gameStateService.SetWin();
        }

        _finishRoutine = null;
    }

    private void OnVehicleDied()
    {
        LoseGame();
    }

    private void LoseGame()
    {
        if (_gameStateService == null)
            return;

        if (_gameStateService.CurrentState == GameState.Win ||
            _gameStateService.CurrentState == GameState.Lose)
            return;

        IsLevelFinishing = false;
        _carController?.StopMovement();

        if (_finishRoutine != null)
        {
            StopCoroutine(_finishRoutine);
            _finishRoutine = null;
        }

        _gameStateService.SetLose();
    }
}