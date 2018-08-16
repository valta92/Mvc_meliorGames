using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ArcherModel {

    public ReactiveProperty<float> attackRange { get; private set; }
    public ReactiveProperty<int> damage { get; private set; }
    public ReactiveProperty<bool> canAttack { get; private set; }
    public ReactiveProperty<float> attackRate { get; private set; }
    public ReactiveProperty<int> level { get; private set; }

    public Vector3 arrowStartPosition = new Vector3(-2.099f, 0.079f, 0);
    public string enemyTag = "Enemy";


    public ArcherModel(ArcherContainer container){

        attackRange = new ReactiveProperty<float>(container.attackRange);
        damage = new ReactiveProperty<int>(container.damage);
        attackRate = new ReactiveProperty<float>(container.attackRate);
        canAttack = new ReactiveProperty<bool>(true);
        level = new ReactiveProperty<int>(container.level);
    }

    public void Attack()
    {
        canAttack.Value = false;
        Observable.Timer
                  (TimeSpan.FromSeconds(attackRate.Value))
                  .Subscribe(x => canAttack.Value = true);
    }
}



public class ArcherContainer
{
    public float attackRange;
    public int damage;
    public float attackRate;
    public int level;


    public ArcherContainer(float attackRange, int damage, float attackRate,int level){
        this.attackRate = attackRate;
        this.damage = damage;
        this.attackRange = attackRange;
        this.level = level;
    }
}