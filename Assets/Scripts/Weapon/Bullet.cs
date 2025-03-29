using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header(" Components ")]
    private Rigidbody2D rb;

    [Header(" Elements ")]
    private RangeWeapon rangeWeapon;

    [Header(" Settings ")]
    [SerializeField] private LayerMask whatIsEnemy;
    private int damage;
    private bool isCriticalHit;
    private Enemy target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Configure(RangeWeapon rangeWeapon)
    {
        this.rangeWeapon = rangeWeapon;
    }

    public void Reload()
    {
        target = null;

        rb.velocity = Vector2.zero;
    }

    public void Shoot(int damage, Vector3 direction, float bulletSpeed, bool isCriticalHit)
    {
        Invoke(nameof(Release), 1f);

        this.damage = damage;
        rb.velocity = direction * bulletSpeed;
        this.isCriticalHit = isCriticalHit;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null)
            return;

        if (IsInLayerMask(collision.gameObject.layer, whatIsEnemy))
        {
            target = collision.GetComponent<Enemy>();

            CancelInvoke();

            Attack(target);
            Release();
        }
    }

    private void Release()
    {
        if (!gameObject.activeSelf)
            return;

        rangeWeapon.ReleaseBullet(this);
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit);
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & 1 << layer) != 0;
    }
}
