using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

public class GameUIPresenter : MonoBehaviour , IInitializable{

    public GameUIView view;
    [Inject]
    SpawnManager spawnManager;
    [Inject]
    GameManager gameManager;
    [Inject]
    FortressPresenter fortressPresenter;
    CompositeDisposable disposables = new CompositeDisposable();

    [Inject]
    ShopUIPresenter shopUI;

    [Inject]
    public void Initialize()
    {
        view.pauseButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                gameManager.SetPause(!gameManager.isPaused);
            })
            .AddTo(this);

        view.exitButtonCompleted.OnClickAsObservable()
            .Subscribe(_ => {
                ShopTime();
        })
            .AddTo(this);

        view.exitButtonFailed.OnClickAsObservable()
            .Subscribe(_ => SceneManager.LoadScene("MainMenu"))
            .AddTo(this);

        spawnManager.leftActiveEnemies.ObserveEveryValueChanged(x => x.Value)
             .Subscribe(count => view.SetTextCountEnemies(count))
             .AddTo(disposables);

        fortressPresenter.model.currentHealth.ObserveEveryValueChanged(x => x.Value)
             .Subscribe(count => view.SetFortressHealth((float)count / (float)fortressPresenter.model.maxHealth.Value * 100))
             .AddTo(disposables);

        gameManager.currentWaveNumber.ObserveEveryValueChanged(x => x.Value)
             .Subscribe(count => view.SetWave(count))
            .AddTo(disposables);

        gameManager.currentGold.ObserveEveryValueChanged(x => x.Value)
            .Subscribe(count => view.SetTextGold(count))
            .AddTo(disposables);

        view.nextWaveButton.OnClickAsObservable()
            .Subscribe(_ => gameManager.StartNextWave())
            .AddTo(this);
    }


    public void SetLevelCleared(bool value){
        view.setLevelClearedWindow(value);
    }

    public void SetGameUI(bool value){
        view.setGameUI(value);
    }

    public void SetLevelFailed(bool value){
        view.setLevelFailedWindow(value);
    }

    public void ShopTime()
    {
        view.setLevelClearedWindow(false);
        if(shopUI != null)
            shopUI.gameObject.SetActive(true);
        view.SetNextWaveBUtton(true);
        view.setGameUI(true);
        view.SetNextWaveBUtton(true);
    }

    public void StartWave()
    {
        view.SetNextWaveBUtton(false);
        view.setGameUI(true);
    }


}
