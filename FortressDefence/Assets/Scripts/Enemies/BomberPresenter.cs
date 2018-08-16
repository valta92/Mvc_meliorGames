using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public class BomberPresenter : SkeletonPresenter {

    [Inject]
    BombProjectile.Pool pool;

	public override void AnimationHit(){

        BomberView v = (BomberView)view;
        BombProjectile bomb = pool.Spawn(curTarget.gameObject, 60, 5, this.gameObject);
        bomb.transform.position = v.bombPosition.position;
        bomb.transform.rotation = Quaternion.identity;

    }
}
