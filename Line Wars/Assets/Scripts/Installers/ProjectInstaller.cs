using DefaultNamespace;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private NetworkManagerInstantiator networkManagerInstantiator;

    public override void InstallBindings()
    {
        var networkManager = networkManagerInstantiator.Instantiate();

        Container.Bind<NetworkManager>().FromInstance(networkManager);
        Container.Bind<IHostManager>().To<HostManager>().FromComponentsInHierarchy().AsSingle();
    }
}