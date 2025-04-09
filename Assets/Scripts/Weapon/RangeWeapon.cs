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
    private Vector3 originalUpVector;

    [Header(" Pooling ")]
    private ObjectPool<Bullet> bulletPool;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        originalUpVector = transform.up;
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
        Vector3 targetUpVector = originalUpVector;

        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;

            ManageAttack();
            return;
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, aimLerp * Time.deltaTime);
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

        newBullet.Shoot(damage, transform.up, bulletSpeed, isCriticalHit);
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        SetupStats();

        damage = Mathf.RoundToInt(damage * (1 + playerStatsManager.GetStatValue(Stat.Attack) / 100));
        attackDelay /= (1 + playerStatsManager.GetStatValue(Stat.AttackSpeed) / 100);

        criticalChance = Mathf.RoundToInt(criticalChance * (1 + playerStatsManager.GetStatValue(Stat.CriticalChance) / 100));
        criticalPercent += Mathf.RoundToInt(playerStatsManager.GetStatValue(Stat.CriticalPercent));

        attackRange += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}
