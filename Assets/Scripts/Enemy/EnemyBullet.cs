using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header(" Elements ")]
    private Rigidbody2D rb;
    private RangeEnemyAttack rangeEnemyAttack;

    [Header(" Settings ")]
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(IEReleaseBullet());
    }

    private IEnumerator IEReleaseBullet()
    {
        yield return new WaitForSeconds(3f);

        if(gameObject.activeSelf)
            rangeEnemyAttack.ReleaseBullet(this);
    }

    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }

    public void Shoot(int damage, Vector3 direction, float moveSpeed)
    {
        this.damage = damage;
        rb.velocity = direction * moveSpeed;
    }

    public void Reload()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }
}
