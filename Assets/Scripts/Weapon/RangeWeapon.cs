using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeWeapon : Weapon
{
    [Header(" Elements ")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootPoint;

    [Header(" Settings ")]
    [SerializeField] private float bulletSpeed;
    

    [Header(" Pooling ")]
    private ObjectPool<Bullet> bulletPool;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        bulletPool = new ObjectPool<Bullet>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    protected override void Update()
    {
        base.Update();

        AutoAim();
    }

    private Bullet CreateFunc()
    {
        Bullet newBullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        newBullet.Configure(this);

        return newBullet;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootPoint.position;

        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
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

    private void ManageAttack()
    {
        if(attackTimer <= 0)
        {
            attackTimer = attackDelay;
            Shoot();
        }
    }

    private void Shoot()
    {
        Bullet newBullet = bulletPool.Get();

        int damage = GetDamage(out bool isCriticalHit);

        newBullet.Shoot(damage, transform.right, bulletSpeed, isCriticalHit);

        AudioManager.instance.PlaySFX(1);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        SetupStats();

        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Damage) / 100));
        attackDelay /= (1 + playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += Mathf.RoundToInt(playerStatsManager.GetStatValue(Stat.CriticalPercent));

        attackRange += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}
