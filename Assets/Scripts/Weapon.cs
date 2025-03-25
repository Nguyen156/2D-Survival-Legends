using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private enum State { Idle, Attack }
    private State state;

    [Header(" Components ")]
    private Animator anim;

    [Header(" Elements ")]
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;

    [Header(" Settings ")]
    [SerializeField] private float range;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header(" Attack ")]
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    private float attackTimer;
    private bool canDealDamage;
    private List<Enemy> damagedEnemies = new List<Enemy>();

    [Header(" Animations ")]
    [SerializeField] private float aimLerp;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        WaitToAttack();

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
        state = State.Attack;

        damagedEnemies.Clear();
    }

    private void Attacking()
    {
        canDealDamage = true;
    }

    private void StopAttack()
    {
        state = State.Idle;
        canDealDamage = false;
        damagedEnemies.Clear();
    }

    private void Attack()
    {
        if (!canDealDamage)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, whatIsEnemy);

        foreach(var hit  in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                if (!damagedEnemies.Contains(enemy))
                {
                    enemy.TakeDamage(damage);
                    damagedEnemies.Add(enemy);
                }
            }
        }

    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector3 targetUpVector = Vector3.up;

        if(closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);
    }

    private void ManageAttack()
    {
        if(attackTimer <= 0 && GetClosestEnemy() != null)
        {
            StartAttack();
            attackTimer = attackDelay;
        }
    }

    private void WaitToAttack()
    {
        if(attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    private Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, whatIsEnemy);

        if (colliders.Length <= 0)
            return null;

        float minDistance = range;

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemy;
                minDistance = distanceToEnemy;
            }
        }

        return closestEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
