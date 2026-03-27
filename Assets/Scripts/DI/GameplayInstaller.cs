using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private VehicleHealth vehicleHealth;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CarController carController;
    [SerializeField] private CarFollowCamera carFollowCamera;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LevelProgressTracker levelProgressTracker;
    [SerializeField] private RoadGenerator roadGenerator;

    public override void InstallBindings()
    {
        Container.Bind<VehicleHealth>().FromInstance(vehicleHealth).AsSingle();
        Container.Bind<EnemySpawner>().FromInstance(enemySpawner).AsSingle();
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        Container.Bind<CarController>().FromInstance(carController).AsSingle();
        Container.Bind<CarFollowCamera>().FromInstance(carFollowCamera).AsSingle();
        Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle();
        Container.Bind<LevelProgressTracker>().FromInstance(levelProgressTracker).AsSingle();
        Container.Bind<RoadGenerator>().FromInstance(roadGenerator).AsSingle();
        Container.Bind<EnemyEncounterTracker>()
    .FromComponentInHierarchy()
    .AsSingle();

    }
}