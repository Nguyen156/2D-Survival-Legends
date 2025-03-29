using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header(" Components ")]
    protected Animator anim;

    [Header(" Settings ")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask whatIsEnemy;

    [Header(" Attack ")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    protected float attackTimer;
    

    [Header(" Animations ")]
    [SerializeField] protected float aimLerp;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        WaitToAttack();
    }

    private void WaitToAttack()
    {
        if(attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }

    public int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false;

        if(Random.Range(0, 100) <= 50)
        {
            isCriticalHit = true;
            return damage * 2;
        }

        return damage;
    }

    public Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsEnemy);

        if (colliders.Length <= 0)
            return null;

        float minDistance = attackRange;

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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
