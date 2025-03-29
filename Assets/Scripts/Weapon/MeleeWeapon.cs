using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    private enum State { Idle, Attack }
    private State state;

    [Header(" Elements ")]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private Collider2D attackCollider;

    [Header(" Attack Info ")]
    private bool canDealDamage;

    private List<Enemy> damagedEnemies = new List<Enemy>();

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Idle:
                AutoAim();
                ManageAttack();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    private void StartAttack()
    {
        anim.Play("Attack");
        canDealDamage = true;

        state = State.Attack;

        damagedEnemies.Clear();
    }

    private void StopDealDamage()
    {
        canDealDamage = false;
    }

    private void StopAttack()
    {
        state = State.Idle;

        damagedEnemies.Clear();
    }

    private void ManageAttack()
    {
        if (attackTimer <= 0 && GetClosestEnemy() != null)
        {
            StartAttack();
            attackTimer = attackDelay;
        }
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector3 targetUpVector = Vector3.up;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);
    }

    private void Attack()
    {
        if (!canDealDamage)
            return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll
            (attackCheck.position, attackCollider.bounds.size,
            attackCheck.localEulerAngles.z, whatIsEnemy);

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                if (!damagedEnemies.Contains(enemy))
                {
                    int damage = GetDamage(out bool isCriticalHit);

                    enemy.TakeDamage(damage, isCriticalHit);
                    damagedEnemies.Add(enemy);
                }
            }
        }

    }
}
