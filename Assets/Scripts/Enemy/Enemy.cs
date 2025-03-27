using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header(" Components ")]
    protected EnemyMovement movement;
    protected Collider2D cd;

    [Header(" Elements ")]
    protected Player player;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem deathVFX;

    [Header(" Spawn Sequence Info")]
    [SerializeField] private SpriteRenderer enemySr;
    [SerializeField] private SpriteRenderer spawnIndicatorSr;
    protected bool hasSpawned;

    [Header(" Health ")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header(" Attack Info ")]
    [SerializeField] protected float attackRange;

    [Header(" Actions ")]
    public static Action<int, Vector2> OnDamageTaken;

    [Header(" DEBUG ")]
    [SerializeField] private bool gizmos;

    protected virtual void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        cd = GetComponent<Collider2D>();
        
        player = FindFirstObjectByType<Player>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(player == null)
            Destroy(gameObject);
       
        health = maxHealth;

        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    private IEnumerator SpawnCoroutine()
    {
        SetRendererVisibility(false);
        spawnIndicatorSr.transform.Rotate(0, 0, UnityEngine.Random.Range(0, 90));

        yield return new WaitForSeconds(0.2f);

        float seconds = 0.3f;
        int loopTimes = (int)((seconds - 0.1) / 0.05);

        for (int i = 0; i < loopTimes; i++)
        {
            spawnIndicatorSr.color = new Color(1, 0, 0, 0.5f);

            yield return new WaitForSeconds(seconds);

            spawnIndicatorSr.color = Color.red;
            
            yield return new WaitForSeconds(seconds);

            seconds -= 0.05f;
        }

        SpawnSequenceFinished();
    }

    private void SpawnSequenceFinished()
    {
        SetRendererVisibility();

        hasSpawned = true;

        cd.enabled = true;

        movement.StorePlayer(player);
    }

    private void SetRendererVisibility(bool visibility = true)
    {
        enemySr.enabled = visibility;
        spawnIndicatorSr.enabled = !visibility;
    }
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(health, damage);
        health -= realDamage;

        OnDamageTaken?.Invoke(damage, transform.position);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        deathVFX.transform.parent = null;
        deathVFX.Play();

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
