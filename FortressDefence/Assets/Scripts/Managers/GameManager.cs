using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UniRx;

public class GameManager : IInitializable
{

    public bool isPaused { get { return (Time.timeScale > 0) ? false : true; } }


    public ReactiveProperty<int> currentGold { get; private set; }
    public ReactiveProperty<int> currentWaveNumber { get; private set; }
    public int countWaves { get { return settings.waves.Count; } }
    private SpawnManager.Settings currentWave;

    [Inject]
    private SpawnManager spawnManager;
    [Inject]
    private GameUIPresenter uiManager;
    [Inject]
    private Settings settings;
    [Inject]
    private ShopUIPresenter shopUI;

    [Inject]
    private FortressPresenter fortress;

    [Inject]
    public void Initialize()
    {
        currentGold = new ReactiveProperty<int>(settings.currentGold);
        currentWave = settings.waves[0];
        currentWaveNumber = new ReactiveProperty<int>(0);
        StartGame();
    }

    private void StartGame()
    {
        SetPause(false);
        uiManager.ShopTime();
        shopUI.gameObject.SetActive(true);

    }

    public void GameOver()
    {
        SetPause(true);
        uiManager.SetLevelFailed(true);
        uiManager.SetGameUI(false);
    }

    public void WaveComplete()
    {
        fortress.model.currentHealth.Value = fortress.model.maxHealth.Value;
        uiManager.SetLevelCleared(true);
        uiManager.SetGameUI(false);

    }

    public void SetPause(bool value)
    {
        float scale = (value) ? 0 : 1;
        Time.timeScale = scale;
    }


    public void StartNextWave(){
        if(currentWave.enemies.Count != currentWaveNumber.Value){
            fortress.model.currentHealth.Value = fortress.model.maxHealth.Value;
            currentWaveNumber.Value++;
            spawnManager.StartWave(settings.waves[currentWaveNumber.Value - 1]);
            currentWave = settings.waves[currentWaveNumber.Value];
            uiManager.StartWave();
            shopUI.gameObject.SetActive(false);
        }

    }

    public bool AddGold(int value)
    {
        if ((currentGold.Value + value) < 1)
            return false;

        currentGold.Value += value;
        return true;
    }


    [System.Serializable]
    public class Settings{
        public List<SpawnManager.Settings> waves = new List<SpawnManager.Settings>();
        public int currentGold;
        public int fortHealth;
    }
}
