using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header(" Settings ")]
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    private float attackTimer;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!hasSpawned)
            return;

        movement.FollowPlayer();

        if (attackTimer <= 0)
            TryAttack();
        else
            Wait();
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
            Attack();
    }

    private void Wait()
    {
        attackTimer -= Time.deltaTime;
    }

    private void Attack()
    {
        attackTimer = attackDelay;

        player.TakeDamage(damage);
    }
}
