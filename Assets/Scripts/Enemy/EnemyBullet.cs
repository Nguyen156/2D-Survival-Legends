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
    [SerializeField] private float angularSpeed;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        WaveManager.OnWaveCompleted += WaveCompletedCallback;
    }

    private void OnDestroy()
    {
        WaveManager.OnWaveCompleted -= WaveCompletedCallback;
    }

    private IEnumerator IEReleaseBullet()
    {
        yield return new WaitForSeconds(5f);

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

        if (Mathf.Abs(direction.x + 1) < 0.01f)
            direction.y += .01f;

        rb.velocity = direction * moveSpeed;

        rb.angularVelocity = angularSpeed;

        StartCoroutine(IEReleaseBullet());
    }

    public void Reload()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);

            rangeEnemyAttack.ReleaseBullet(this);
        }
    }

    private void WaveCompletedCallback()
    {
        if (gameObject.activeSelf)
            rangeEnemyAttack.ReleaseBullet(this);
    }
}
