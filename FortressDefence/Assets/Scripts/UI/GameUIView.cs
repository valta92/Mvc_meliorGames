using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameUIView : MonoBehaviour {

    public Button abillityButton;
    public Button pauseButton;
    public Button settingsButton;
    public RectTransform levelCleared;
    public RectTransform levelFailed;
    public Button exitButtonFailed;
    public Button exitButtonCompleted;
    public Text countEnemies;
    public Image fortressHealth;
    public RectTransform fortressHealthBar;
    public RectTransform leftEnemiesBar;
    public Text goldText;
    public RectTransform goldBar;
    public RectTransform waveBar;
    public Text waveText;
    public Button nextWaveButton;


    private float smoothSpeed = 2;

    CompositeDisposable disposables = new CompositeDisposable();

    public void setGameUI(bool value)
    {
        abillityButton.gameObject.SetActive(value);
        pauseButton.gameObject.SetActive(value);
        settingsButton.gameObject.SetActive(value);
        fortressHealthBar.gameObject.SetActive(value);
        leftEnemiesBar.gameObject.SetActive(value);
        goldBar.gameObject.SetActive(value);
        fortressHealth.gameObject.SetActive(value);
        waveBar.gameObject.SetActive(value);
    }

    public void setLevelClearedWindow(bool value)
    {
        levelCleared.gameObject.SetActive(value);
    }

    public void setLevelFailedWindow(bool value)
    {
        levelFailed.gameObject.SetActive(value);
    }

    public void setSettingsWindow(bool value)
    {
        
    }

    public void setShopWindow(bool value)
    {
        
    }

    public void SetNextWaveBUtton(bool value)
    {
        nextWaveButton.gameObject.SetActive(value);
    }

    public void SetTextCountEnemies(int value)
    {
        Debug.Log(value);
        countEnemies.text = "LeftEnemies : " + value.ToString();
    }

    public void SetFortressHealth(float percent)
    {

        Observable.FromCoroutine(_ => SmoothFill(percent / 100))
                  .Subscribe()
                  .AddTo(disposables);
    }


    public void SetTextGold(int value)
    {
        goldText.text = "Gold : " + value.ToString();
    }

    public void SetWave(int value)
    {
        waveText.text = "Wave : " + value.ToString();
    }

	private void OnDisable()
	{
        disposables.Dispose();
	}

	private IEnumerator SmoothFill(float targetValue)
    {
        float currentValue = fortressHealth.fillAmount;

        while (!Mathf.Approximately(currentValue, targetValue))
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * smoothSpeed);
            fortressHealth.fillAmount = currentValue;
            yield return new WaitForEndOfFrame();
        }
    }
}
