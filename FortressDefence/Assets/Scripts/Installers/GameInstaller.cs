using UnityEngine;
using Zenject;
using System.ComponentModel;

public class GameInstaller : MonoInstaller<GameInstaller>
{


    public GameObject spawnManagerPrefab;
    public GameObject gameUIPresenter;
    public GameObject fortressPresenterGameobject;
    public GameObject shopUIPresenter;



    [Inject]
    private PrefabSettings prefabSettings;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle();
        Container.Bind<SpawnManager>().FromComponentOn(spawnManagerPrefab).AsSingle();
        Container.Bind<GameUIPresenter>().FromComponentOn(gameUIPresenter).AsSingle();
        Container.Bind<FortressPresenter>().FromComponentOn(fortressPresenterGameobject).AsSingle();
        Container.BindInterfacesAndSelfTo<ShopUIPresenter>().FromComponentOn(shopUIPresenter).AsSingle();
        Container.BindInterfacesTo<ArcherPresenter>().AsTransient();

        InstallMemoryPools();
    }

    void InstallMemoryPools()
    {
        Container.BindMemoryPool<SkeletonPresenter, SkeletonPresenter.Pool>()
            .WithId("bat")
            .WithInitialSize(10)
        .FromComponentInNewPrefab(prefabSettings.batPrefab)
        .UnderTransformGroup("PoolRoot");
       

        Container.BindMemoryPool<SkeletonPresenter, SkeletonPresenter.Pool>()
            .WithId("onager")
            .WithInitialSize(10)
        .FromComponentInNewPrefab(prefabSettings.onagerPrefab)
            .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<SkeletonPresenter, SkeletonPresenter.Pool>()
            .WithId("bomber")
            .WithInitialSize(10)
        .FromComponentInNewPrefab(prefabSettings.bomberPrefab)
            .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<ArrowProjectile, ArrowProjectile.Pool>()
            .WithInitialSize(20)
        .FromComponentInNewPrefab(prefabSettings.arrowProjectile)
        .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<BombProjectile, BombProjectile.Pool>()
            .WithInitialSize(20)
        .FromComponentInNewPrefab(prefabSettings.bombProjectile)
        .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<ArcherPresenter, ArcherPresenter.Pool>()
            .WithId("archer1")
            .WithInitialSize(3)
        .FromComponentInNewPrefab(prefabSettings.archer1Prefab)
        .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<ArcherPresenter, ArcherPresenter.Pool>()
            .WithId("archer2")
            .WithInitialSize(3)
        .FromComponentInNewPrefab(prefabSettings.archer2Prefab)
        .UnderTransformGroup("PoolRoot");

        Container.BindMemoryPool<ArcherPresenter, ArcherPresenter.Pool>()
            .WithId("archer3")
            .WithInitialSize(3)
        .FromComponentInNewPrefab(prefabSettings.archer3Prefab)
        .UnderTransformGroup("PoolRoot");
    }

    [System.Serializable]
    public class PrefabSettings
    {
        public GameObject batPrefab;
        public GameObject bomberPrefab;
        public GameObject onagerPrefab;
        public GameObject arrowProjectile;
        public GameObject bombProjectile;
        public GameObject archer1Prefab;
        public GameObject archer2Prefab;
        public GameObject archer3Prefab;
    }
}