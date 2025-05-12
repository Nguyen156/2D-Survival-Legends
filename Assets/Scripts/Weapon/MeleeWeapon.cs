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

        AudioManager.instance.PlaySFX(10);
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
        if (attackTimer <= 0)
        {
            StartAttack();
            attackTimer = attackDelay;
        }
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        bool facingRight = Player.instance.FacingRight;
        Vector3 targetDir = facingRight ? Vector3.right : Vector3.left;

        if (closestEnemy != null)
        {
            targetDir = (closestEnemy.transform.position - transform.position).normalized;
            transform.right = targetDir;

            HandleFip(closestEnemy.transform.position.x);

            ManageAttack();
            return;
        }

        //transform.right = Vector3.Lerp(transform.right, targetDir, aimLerp * Time.deltaTime);
        
        transform.right = targetDir;
        HandleFip(Mathf.Infinity);
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

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        SetupStats();

        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Damage) / 100));
        Debug.Log(damage);
        attackDelay /= (1 + playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += Mathf.RoundToInt(playerStatsManager.GetStatValue(Stat.CriticalPercent));
    }
}
