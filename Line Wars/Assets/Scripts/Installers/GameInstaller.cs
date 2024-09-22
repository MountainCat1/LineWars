using Client;
using Managers;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GridGenerator>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IPathfinding>().To<Pathfinding>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IInputManager>().To<InputManager>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IInputMapper>().To<InputMapper>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<ICreatureSpawner>().To<CreatureSpawner>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IBuildingManager>().To<BuildingManager>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IPlayerManager>().To<PlayerManager>().FromComponentsInHierarchy().AsSingle();
    }
}