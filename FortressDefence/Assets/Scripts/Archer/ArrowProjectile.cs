using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using Zenject;

public class ArrowProjectile : MonoBehaviour , IDisposable{

    public GameObject curTarget;
    public int damage;
    public float rotationSpeed;
    public float moveSpeed;
    public GameObject initiator;

    private Vector2 lastPositionTarget;

    CompositeDisposable disposables = new CompositeDisposable();


    [Inject]
    Pool pool;


	public void CreateProjectile(GameObject target, int damage, GameObject initiator){

        curTarget = target;
        this.damage = damage;
        this.initiator = initiator;
        ;

        Observable.EveryUpdate()
                  .Subscribe(_ => Move())
                  .AddTo(disposables);

        curTarget.GetComponent<ObservableEnableTrigger>().OnDisableAsObservable()
                 .Subscribe(_ => OnDisableTarget()).AddTo(disposables);
              
    }

    void OnDisableTarget()
    {
        lastPositionTarget = curTarget.transform.position;
    }

    private void Move()
    {
        Vector3 targetPos = Vector3.zero;

        if (curTarget == null || curTarget.activeSelf == false)
        {
            if (lastPositionTarget != Vector2.zero){
                targetPos = lastPositionTarget;
            }
        }
        else
            targetPos = curTarget.transform.position;

        transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        Vector3 vectorToTarget = transform.position - targetPos;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);


        if (Vector2.Distance(transform.position, targetPos) < 2)
        {
            if (curTarget != null)
            {
                IGetHit hit = curTarget.GetComponent<IGetHit>();
                if (hit != null)
                {
                    hit.GetHit(damage, initiator);
                }
            }

            Dispose();
        }
    }

	private void OnDisable()
	{
        disposables.Clear();
	}

	public void Dispose()
    {
        disposables.Clear();
        pool.Despawn(this);
    }

    public class Pool : MonoMemoryPool<GameObject, int, GameObject, ArrowProjectile>
    {
        protected override void Reinitialize(GameObject p1, int p2, GameObject p3, ArrowProjectile item)
		{
            item.CreateProjectile(p1, p2, p3);
		}
	}
}
