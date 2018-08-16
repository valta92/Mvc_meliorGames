using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

public class SpawnManager : MonoBehaviour {

    public GameObject BatPrefab;
    public GameObject OnagerPrefab;
    public GameObject BomberPrefab;

    public Transform highestPoint;
    public Transform lowestPoint;


    public int maxEnemiesToSpawn;
    public int leftEnemiesToSpawn;


    public List<GameObject> activeEnemies = new List<GameObject>();
    public ReactiveProperty<int> leftActiveEnemies;

    public float timeToCreateEnemies;

    CompositeDisposable disposables = new CompositeDisposable();

    [Inject]
    GameManager manager;

    [Inject(Id = "bat")]
    SkeletonPresenter.Pool batPool;

    [Inject(Id = "onager")]
    SkeletonPresenter.Pool onagerPool;

    [Inject(Id = "bomber")]
    SkeletonPresenter.Pool bomberPool;

    public void StartWave(Settings settings){

        activeEnemies.Clear();
        maxEnemiesToSpawn = settings.enemies.Count;
        timeToCreateEnemies = settings.time;
        leftActiveEnemies = new ReactiveProperty<int>(settings.enemies.Count);

        Observable.FromCoroutine(_ => SpawnEnemies(settings)).Subscribe().AddTo(disposables);
    }

	private void OnDisable()
	{
        disposables.Clear();
	}

	public void RemoveEnemy(GameObject enemy)
    {
        if(activeEnemies.Contains(enemy)){
            activeEnemies.Remove(enemy);
            leftActiveEnemies.Value--;
        }

        if (leftActiveEnemies.Value == 0)
            manager.WaveComplete();
            
    }

    private IEnumerator SpawnEnemies(Settings settings){
        leftEnemiesToSpawn = maxEnemiesToSpawn;

        while(leftEnemiesToSpawn > 0){

            foreach(var enemy in settings.enemies)
            {
                SkeletonPresenter.Pool pool;
                switch(enemy.idName)
                {
                    case "bat": pool = batPool;
                        break;
                    case "onager": pool = onagerPool;
                        break;
                    case "bomber": pool = bomberPool;
                        break;
                    default : pool = batPool;
                        break;
                }

                SkeletonPresenter obj = pool.Spawn(enemy.stats);
                obj.transform.position = new Vector2(highestPoint.position.x, Random.Range(highestPoint.position.y, lowestPoint.position.y));
                obj.transform.rotation = Quaternion.identity;
                obj.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(obj.transform.position.y * 100f) * -1;
                obj.GetComponent<ObservableEnableTrigger>()
                        .OnDisableAsObservable()
                   .Subscribe(_ => RemoveEnemy(obj.gameObject))
                        .AddTo(disposables);
                activeEnemies.Add(obj.gameObject);
                leftEnemiesToSpawn--;  
                yield return new WaitForSeconds(timeToCreateEnemies);
             }
             
        }
    }

    [System.Serializable]
    public class Settings
    {
        public List<EnemyContainer> enemies = new List<EnemyContainer>();
        public float time;
    }
}


[System.Serializable]
public class EnemyContainer
{
    public string idName;
    public SkeletonContainer stats;
}
