using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using Zenject;
using System;

public class ArcherPresenter : MonoBehaviour   {


    public enum State
    {
        Idle,
        Attack
    }

    public State state;

    public ArcherView view;
    public ArcherModel model;

    public GameObject curTarget;
    public List<GameObject> targetsInTrigger = new List<GameObject>();

    [Inject]
    private ArrowProjectile.Pool arrowPool;

    private ObservableTrigger2DTrigger trigger;
    CompositeDisposable disposables = new CompositeDisposable();

    public void CreateArcher(ArcherContainer container){

        model = new ArcherModel(container);
        state = State.Idle;
        trigger = GetComponent<ObservableTrigger2DTrigger>();
        trigger
            .OnTriggerEnter2DAsObservable()
            .Subscribe(x => TriggerEnter2D(x))
            .AddTo(disposables);


        trigger
            .OnTriggerExit2DAsObservable()
            .Subscribe(_ => TriggerExit2D(_))
            .AddTo(disposables);

        Observable.EveryUpdate()
                  .Subscribe(_ => StatesUpdate())
                  .AddTo(disposables);
    }

	private void OnDisable()
	{
        model = null;
        targetsInTrigger.Clear();
        curTarget = null;
        disposables.Clear();
	}


	private void StatesUpdate(){
        
        switch (state)
        {
            case State.Idle: break;
            case State.Attack:
                if (curTarget == null)
                    return;
                if (model.canAttack.Value)
                {
                    model.Attack();
                    view.Shoot();
                }
                break;
        }
    }

    void RemoveTarget(GameObject enemy){
        if (targetsInTrigger.Contains(enemy.gameObject)){
            targetsInTrigger.Remove(enemy.gameObject);
        }
        if(enemy == curTarget)
        {
            curTarget = null;
            if (targetsInTrigger.Count != 0)
            {
                int random = UnityEngine.Random.Range(0, targetsInTrigger.Count - 1);
                SetTarget(targetsInTrigger[random]);
            }
        }

    }

    void SetTarget(GameObject target){
        curTarget = target;
        state = State.Attack;
    }

    // AnimationEvent
    public void OnAnimationShoot()
    {
        var projectile = arrowPool.Spawn(curTarget, model.damage.Value, this.gameObject);
        projectile.transform.position = transform.position + model.arrowStartPosition;
        projectile.transform.rotation = Quaternion.identity;
    }

    private void TriggerEnter2D(Collider2D other)
	{
        if(other.tag == model.enemyTag){
            targetsInTrigger.Add(other.gameObject);

            other.gameObject.GetComponent<ObservableEnableTrigger>()
                .OnDisableAsObservable()
                .Subscribe(x => RemoveTarget(other.gameObject))
                 .AddTo(disposables);

            if(curTarget == null){
                SetTarget(other.gameObject);
            }
        }
	}

    private void TriggerExit2D(Collider2D other)
    {
        if (other.tag == model.enemyTag){
            RemoveTarget(other.gameObject);
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public class Pool : MonoMemoryPool<ArcherContainer,ArcherPresenter>
    {
        protected override void Reinitialize(ArcherContainer container,ArcherPresenter target)
        {
            target.CreateArcher(container);
        }

		protected override void OnDespawned(ArcherPresenter item)
		{
            base.OnDespawned(item);
		}
	}

   
}
