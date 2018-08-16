using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx.Triggers;
using UniRx;
using System;

public class ShopUIPresenter : MonoBehaviour, IInitializable
{

    public ShopUIView view;
    [Inject(Id = "archer1")]
    ArcherPresenter.Pool archer1Pool;

    [Inject(Id = "archer2")]
    ArcherPresenter.Pool archer2Pool;

    [Inject(Id = "archer3")]
    ArcherPresenter.Pool archer3Pool;

    [Inject]
    private FortressPresenter fortress;
    [Inject]
    private GameManager manager;

    [Inject]
    private FortressPresenter fort;

    CompositeDisposable disposables = new CompositeDisposable();

    public void Initialize()
    {
        view.upButtonArcher1.OnClickAsObservable()
            .Subscribe(_ => BuyArcher(1))
            .AddTo(disposables);

        view.upButtonArcher2.OnClickAsObservable()
            .Subscribe(_ => BuyArcher(2))
            .AddTo(disposables);

        view.upButtonArcher3.OnClickAsObservable()
            .Subscribe(_ => BuyArcher(3))
            .AddTo(disposables);
    }


    public void BuyArcher(int value)
    {


        if (fortress.archers[value - 1].archer == null)
        {

            if (!manager.AddGold(-PriceUpdates.archerLevel1))
                return;


            ArcherPresenter archer = archer1Pool.Spawn(ArcherStats.GetArcherStats1);
            archer.transform.position = fortress.archers[value - 1].rootTransform.position;
            archer.transform.rotation = Quaternion.identity;
            fortress.archers[value - 1].archer = archer;
            return;
        }

        switch (fortress.archers[value - 1].archer.model.level.Value)
        {
            case 1:
                if (!manager.AddGold(-PriceUpdates.archerLevel2))
                    return;
                archer1Pool.Despawn(fortress.archers[value - 1].archer);
                ArcherPresenter archer = archer2Pool.Spawn(ArcherStats.GetArcherStats2);
                archer.transform.position = fortress.archers[value - 1].rootTransform.position;
                archer.transform.rotation = Quaternion.identity;
                fortress.archers[value - 1].archer = archer;
                break;
            case 2:

                if (!manager.AddGold(-PriceUpdates.archerLevel3))
                    return;

                archer2Pool.Despawn(fortress.archers[value - 1].archer);
                ArcherPresenter archerd = archer3Pool.Spawn(ArcherStats.GetArcherStats3);
                archerd.transform.position = fortress.archers[value - 1].rootTransform.position;
                archerd.transform.rotation = Quaternion.identity;
                fortress.archers[value - 1].archer = archerd;
                break;
            default:
                Debug.Log("maxRank");
                return;
        }
    }
}

public class PriceUpdates
{
    public static int archerLevel1 = 20;
    public static int archerLevel2 = 50;
    public static int archerLevel3 = 100;
}
