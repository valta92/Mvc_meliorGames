using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;
using System;

public class BombProjectile : MonoBehaviour , IDisposable
{

    public GameObject currentTarget;
    public GameObject initiator;
    public int damage;

    private Rigidbody2D rb;
    private ObservableTrigger2DTrigger trigger;
    private CompositeDisposable disposables = new CompositeDisposable();

    [Inject]
    Pool pool;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trigger = GetComponent<ObservableTrigger2DTrigger>();
    }

    Vector2 BallisticVel(Transform target, float angle)
    {
        Vector3 dir = target.position - transform.position;
        float h = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += h / Mathf.Tan(a);
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }


    public void CreateProjectile(GameObject target, float angle, int damage, GameObject initiator)
    {
        currentTarget = target;
        this.damage = damage;
        this.initiator = initiator;

        rb.velocity = BallisticVel(target.transform, angle);

        trigger.OnTriggerEnter2DAsObservable()
               .Subscribe(_ => TriggerEnter2D(_));
    }


    void TriggerEnter2D(Collider2D col){


        if(col.gameObject == currentTarget)
        {
            col.gameObject.GetComponent<IGetHit>().GetHit(damage, initiator);
            DestroySelf();
        }
    }


    void DestroySelf()
    {
        Dispose();
        rb.velocity = Vector2.zero;
        disposables.Clear();
        try{
            pool.Despawn(this);
        }
        catch{
            
        }

    }

    public void Dispose()
    {
        
    }



    public class Pool : MonoMemoryPool<GameObject,float, int, GameObject, BombProjectile>
    {
		protected override void Reinitialize(GameObject p1, float p2, int p3, GameObject p4, BombProjectile item)
		{
            Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ => item.CreateProjectile(p1, p2, p3, p4));

		}
	}
}
