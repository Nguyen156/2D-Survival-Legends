using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RangeEnemyAttack : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private EnemyBullet bulletPrefab;
    private Player player;

    [Header(" Settings ")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float attackDelay;
    private float attackTimer;

    [Header(" Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool;

    // Start is called before the first frame update
    void Start()
    {
        bulletPool = new ObjectPool<EnemyBullet>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        Wait();
    }

    private EnemyBullet CreateFunc()
    {
        EnemyBullet newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.Configure(this);

        return newBullet;
    }

    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = transform.position;

        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    private void Wait()
    {
        if(attackTimer > 0)
            attackTimer -= Time.deltaTime;  
    }

    public void AutoAim()
    {
        if (attackTimer <= 0)
        {
            attackTimer = attackDelay;
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        EnemyBullet newBullet = bulletPool.Get();

        newBullet.Shoot(damage, direction, bulletSpeed);
    }
}
