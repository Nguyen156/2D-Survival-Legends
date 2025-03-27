using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
    [Header(" Components ")]
    private RangeEnemyAttack attack;

    protected override void Awake()
    {
        base.Awake();
        attack = GetComponent<RangeEnemyAttack>();
        attack.StorePlayer(player);
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!hasSpawned)
            return;

        ManageAttack();
    }

    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if(distanceToPlayer <= attackRange)
        {
            Attack();
            return;
        }

        movement.FollowPlayer();
    }

    private void Attack()
    {
        attack.AutoAim();
    }
}
