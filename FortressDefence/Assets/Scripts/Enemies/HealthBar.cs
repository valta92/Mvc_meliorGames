using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class HealthBar : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    public float smoothSpeed;

    private string fill = "_Fill";
    CompositeDisposable disposables = new CompositeDisposable();

	void Awake () {

        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    public void ChangeFill(float percent){

        Observable.FromCoroutine(_ => SmoothFill(percent / 100)).Subscribe().AddTo(disposables);

    }

	private void OnDisable()
	{
        disposables.Clear();
	}

	private IEnumerator SmoothFill(float targetValue){

        float currentValue = spriteRenderer.material.GetFloat(fill);

        while(!Mathf.Approximately(currentValue,targetValue)){
            
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * smoothSpeed);
            spriteRenderer.material.SetFloat(fill, currentValue);
            yield return new WaitForEndOfFrame();
        }
    }
}
