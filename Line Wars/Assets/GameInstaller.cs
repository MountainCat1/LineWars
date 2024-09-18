using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<GridGenerator>().FromComponentsInChildren().AsSingle();
        Container.Bind<IPathfinding>().To<Pathfinding>().FromComponentsInChildren().AsSingle();
    }
    
}
