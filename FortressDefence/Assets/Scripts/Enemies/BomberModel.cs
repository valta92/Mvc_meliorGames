using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public class BomberModel : SkeletonModel
{

    public ReactiveProperty<float> attackRange { get; private set; }

    public BomberModel(BomberContainer container) 
        : base(new SkeletonContainer(container.damage,container.attackRate,container.maxHealth,container.moveSpeed,container.gold))
    {
        attackRange = new ReactiveProperty<float>(container.attackRange);
    }
}



