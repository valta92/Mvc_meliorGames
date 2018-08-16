using UnityEngine;
using UniRx;
using Zenject;
using System.Collections;
using System.Collections.Generic;

public class FortressPresenter : MonoBehaviour , IGetHit , IInitializable{

    public FortressView view;
    public FortressModel model;


    [Inject]
    private GameManager gameManager;
    [Inject]
    private Settings settings;

    public List<ArcherSettings> archers = new List<ArcherSettings>();
    CompositeDisposable disposables = new CompositeDisposable();

    [Inject]
    public void Initialize()
    {
        model = new FortressModel(settings.health);
        view.SetHealth(settings.health);
        model.isDestroyed.Where(isDead => isDead == true)
           .Subscribe(_ =>
           {
               gameManager.GameOver();
           });

        model.currentHealth.ObserveEveryValueChanged(x => x.Value)
             .Subscribe(y =>
        {
            view.SetHealth((float)y / (float)model.maxHealth.Value * 100);
        }).AddTo(disposables);
    }

    public int GetCurHealth()
    {
        return model.currentHealth.Value;
    }

    void OnDisable()
    {
        disposables.Clear();
    }

    public void GetHit(int damage, GameObject initiator)
    {
        model.currentHealth.Value-= damage;
    }


    [System.Serializable]
    public class Settings
    {
        public int health;
    }

}

[System.Serializable]
public class ArcherSettings
{
    public ArcherPresenter archer;
    public Transform rootTransform;
}