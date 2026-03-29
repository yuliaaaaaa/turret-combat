using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindServices();
        BindSceneComponents();
    }

    private void BindServices()
    {
        Container.Bind<GameStateService>().AsSingle();
        Container.Bind<SceneReloadService>().AsSingle();
        Container.Bind<EnemyEncounterService>().AsSingle();
        Container.Bind<LevelProgressService>().AsSingle();
    }

    private void BindSceneComponents()
    {
        Container.Bind<LevelFlowCoordinator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameTimeController>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<CarController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<CarFollowCamera>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<VehicleDamageReceiver>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<AudioManager>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<EnemySpawner>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<RoadGenerator>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<FinishWatcher>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<AutoShooter>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<TurretAimController>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<VehicleHealthView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<LevelProgressView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<LevelProgressUpdater>().FromComponentInHierarchy().AsSingle().NonLazy();

        Container.Bind<HudView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PauseMenuView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameOverlayView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<SettingsPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<GameUiCoordinator>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}