using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;

public class SkeletonView : MonoBehaviour {


    public ObservableTrigger2DTrigger trigger;

    [SerializeField]
    private HealthBar HPBar;
    private SpriteRenderer spriteRenderer;

    public SkeletonPresenter presenter;

    private Animator animator;

    private static int deathTriggerHash;
    private static int attackTriggerHash;
    private static int velocityHash;


    CompositeDisposable disposables = new CompositeDisposable();

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        HPBar.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;

        velocityHash = Animator.StringToHash("velocity");
        attackTriggerHash = Animator.StringToHash("attack");
        deathTriggerHash = Animator.StringToHash("death");
    }

	private void OnEnable()
	{
        Observable.EveryUpdate().Subscribe(_ => UpdateAnimator())
                  .AddTo(disposables);
	}

	private void OnDisable()
	{
        disposables.Clear();
	}

	public void SetHP(float percent)
    {
        HPBar.ChangeFill(percent);
    }

    void UpdateAnimator(){
        animator.SetFloat(velocityHash, presenter.GetVelocity());
    }

    public void Death()
    {
        animator.SetTrigger(deathTriggerHash);
    }

    public void Attack()
    {
        animator.SetTrigger(attackTriggerHash);
    }

    IDisposable d;
    public void GetHit(){
        if (d != null)
            d.Dispose();
        
        Observable.FromCoroutine(HitAnimation).Subscribe();
    }

    private IEnumerator HitAnimation(){

        float time = 0;
        float value = 0;

        while(value < 1)
        {
            time += Time.deltaTime / 0.5f;
            value = Mathf.MoveTowards(spriteRenderer.material.GetFloat("_FlashAmount"), 1, time);
            spriteRenderer.material.SetFloat("_FlashAmount", value);
            yield return new WaitForEndOfFrame();
        }

        while (value > 0)
        {
            time += Time.deltaTime / 0.5f;
            value = Mathf.MoveTowards(spriteRenderer.material.GetFloat("_FlashAmount"), 0, time);
            spriteRenderer.material.SetFloat("_FlashAmount", value);
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }
}
