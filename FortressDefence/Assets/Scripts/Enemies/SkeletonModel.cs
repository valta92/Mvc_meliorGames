using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class SkeletonModel{

    public ReactiveProperty<int> currentHealth { get; private set; }
    public ReactiveProperty<int> maxHealth { get; private set; }
    public ReactiveProperty<bool> isDead { get; private set; }
    public ReactiveProperty<float> moveSpeed { get; private set; }
    public ReactiveProperty<bool> canAttack { get; private set; }
    public ReactiveProperty<float> attackRate { get; private set; }
    public ReactiveProperty<int> damage { get; private set; }
    public ReactiveProperty<int> gold { get; private set; }
    public string fortressTag = "Fortress";


    public SkeletonModel(SkeletonContainer container)
    {


        maxHealth = new ReactiveProperty<int>(container.maxHealth);
        currentHealth = new ReactiveProperty<int>(maxHealth.Value);
        moveSpeed = new ReactiveProperty<float>(container.moveSpeed);
        canAttack = new ReactiveProperty<bool>(true);
        attackRate = new ReactiveProperty<float>(container.attackRate);
        isDead = new ReactiveProperty<bool>(false);
        damage = new ReactiveProperty<int>(container.damage);
        gold = new ReactiveProperty<int>(container.gold);

        currentHealth.Where(hp => hp < 1)
                     .Subscribe(_ => isDead.Value = true);
    }

    public virtual void Attack(){
        canAttack.Value = false;
        Observable.Timer
                  (TimeSpan.FromSeconds(attackRate.Value))
                  .Subscribe(x => canAttack.Value = true);
    }
}

[System.Serializable]
public class SkeletonContainer
{
    public int damage;
    public int maxHealth;
    public float moveSpeed;
    public float attackRate;
    public int gold;

    public SkeletonContainer(int damage, float attackRate, int maxHealth, float moveSpeed, int gold)
    {
        this.attackRate = attackRate;
        this.damage = damage;
        this.maxHealth = maxHealth;
        this.moveSpeed = moveSpeed;
        this.gold = gold;
    }
}

[System.Serializable]
public class BomberContainer : SkeletonContainer
{
    public float attackRange;

    public BomberContainer(int damage, float attackRate, int maxHealth, float moveSpeed, float attackRange,int gold) : base(damage, attackRate, maxHealth, moveSpeed,gold)
    {
        this.attackRange = attackRange;
    }
}
