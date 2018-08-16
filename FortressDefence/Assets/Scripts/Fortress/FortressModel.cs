using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FortressModel : MonoBehaviour {

    public ReactiveProperty<int> currentHealth { get; private set; }
    public ReactiveProperty<int> maxHealth { get; private set; }
    public ReactiveProperty<bool> isDestroyed { get; private set; }


    public FortressModel(int maxHP)
    {

        maxHealth = new ReactiveProperty<int>(maxHP);
        currentHealth = new ReactiveProperty<int>(maxHealth.Value);

        isDestroyed = new ReactiveProperty<bool>(false);

        currentHealth.Where(hp => hp < 1)
            .Subscribe(_ =>
            {
            isDestroyed.Value = true;
            });
    }
}
