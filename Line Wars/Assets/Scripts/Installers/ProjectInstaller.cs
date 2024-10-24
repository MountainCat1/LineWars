using DefaultNamespace;
using Managers;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private NetworkManagerInstantiator networkManagerInstantiator;

    public override void InstallBindings()
    {
        var networkManager = networkManagerInstantiator.Instantiate();

        Container.Bind<GameServerSettings>().FromInstance(ScriptableObject.CreateInstance<GameServerSettings>()).AsSingle(); // TODO: Load from file
        Container.Bind<NetworkManager>().FromInstance(networkManager);
        Container.Bind<IPlayerManager>().To<PlayerManager>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IHostManager>().To<HostManager>().FromComponentsInHierarchy().AsSingle();
    }
}