using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [Header(" Components ")]
    private EnemyMovement movement;

    [Header(" Elements ")]
    private Player player;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem deathVFX;

    [Header(" Spawn Sequence Info")]
    [SerializeField] private SpriteRenderer enemySr;
    [SerializeField] private SpriteRenderer spawnIndicatorSr;
    private bool hasSpawned;

    [Header(" Attack Info ")]
    [SerializeField] private int damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDelay;
    private float attackTimer;

    [Header(" DEBUG ")]
    [SerializeField] private bool gizmos;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        player = FindFirstObjectByType<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
            Destroy(gameObject);

        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasSpawned)
            return;

        if (attackTimer <= 0)
            TryAttack();
        else
            Wait();
    }

    private IEnumerator SpawnCoroutine()
    {
        SetRendererVisibility(false);
        spawnIndicatorSr.transform.Rotate(0, 0, Random.Range(0, 90));

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

        movement.StorePlayer(player);
    }

    private void SetRendererVisibility(bool visibility = true)
    {
        enemySr.enabled = visibility;
        spawnIndicatorSr.enabled = !visibility;
    }

    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
            Attack();
    }

    private void Wait()
    {
        attackTimer -= Time.deltaTime;
    }

    private void Attack()
    {
        attackTimer = attackDelay;

        player.TakeDamage(damage);
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
