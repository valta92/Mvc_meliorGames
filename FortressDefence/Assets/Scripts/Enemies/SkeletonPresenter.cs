using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using Zenject;

public class SkeletonPresenter : MonoBehaviour , IGetHit{

    public enum State{
        Idle,
        Walk,
        Attack
    }

    public State state;

    [SerializeField]
    public SkeletonView view;
    public SkeletonModel model;

    public GameObject curTarget;


    [Inject]
    private GameManager gameManager;

    private Rigidbody2D rb;
    CompositeDisposable disposables = new CompositeDisposable();


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void CreateSkeleton(SkeletonContainer container){
        model = new SkeletonModel(container);
        model.currentHealth.ObserveEveryValueChanged(x => x.Value)
             .Subscribe(hp => view.SetHP((float)hp / (float)model.maxHealth.Value * 100))
             .AddTo(disposables);

        model.isDead.Where(isDead => isDead == true)
            .Subscribe(_ =>
            {
                Death();
            }).AddTo(disposables);

        view.trigger
            .OnTriggerEnter2DAsObservable()
            .Subscribe(x => TriggerEnter(x.gameObject))
            .AddTo(disposables);


        Observable.EveryUpdate()
                  .Subscribe(_ => StatesUpdate())
                  .AddTo(disposables);

        state = State.Walk;
    }

	private void OnDisable()
	{
        disposables.Clear();
	}


	// * __ * 
	private void StatesUpdate()
    {
        switch (state)
        {
            case State.Idle: 
                rb.velocity = Vector2.zero;
                break;
            case State.Walk:
                rb.velocity = Vector2.right * Time.deltaTime * model.moveSpeed.Value * 50;
                break;
            case State.Attack:
                rb.velocity = Vector2.zero;
                if (model.canAttack.Value)
                {
                    view.Attack();
                    model.Attack();
                }
                break;
        }
    }

    public void Death(){
        gameManager.AddGold(model.gold.Value);
        state = State.Idle;
        view.Death();

    }


    public float GetVelocity(){
        return rb.velocity.magnitude;
    }

    public virtual void AnimationHit()
    {
        if(curTarget != null)
        {
            IGetHit target = curTarget.GetComponent<IGetHit>();
            if(target != null)
                target.GetHit(model.damage.Value, this.gameObject);
        }
    }

    public void OnAnimationDeathEnded(){
        Destroy(this.gameObject);
    }
   
    protected virtual void TriggerEnter(GameObject other)
	{
        if(other.tag == model.fortressTag){
            curTarget = other.gameObject;
            state = State.Attack;
        }
	}

    public void GetHit(int damage, GameObject initiator)
    {
        model.currentHealth.Value -= damage;
        if(!model.isDead.Value)
            view.GetHit();
    }


    public class Pool : MonoMemoryPool<SkeletonContainer,SkeletonPresenter>
    {
        protected override void Reinitialize(SkeletonContainer item,SkeletonPresenter target)
		{
            target.CreateSkeleton(item);
		}
    }
}
