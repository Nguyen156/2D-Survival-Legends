using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData {  get; private set; }

    [Header(" Components ")]
    protected Animator anim;

    [Header(" Settings ")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask whatIsEnemy;

    public int Level { get; private set; }

    [Header(" Attack ")]
    [SerializeField] protected int damage;
    [SerializeField] protected float attackDelay;
    protected float attackTimer;

    [Header(" Crit ")]
    protected int criticalChance;
    protected int criticalPercent;

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

        if(Random.Range(0, 100) < criticalChance)
        {
            isCriticalHit = true;
            return damage * criticalPercent;
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
    public abstract void UpdateStats(PlayerStatsManager playerStats);

    public void SetupStats()
    {
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData,Level);

        damage          = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay     = 1f / (calculatedStats[Stat.AttackSpeed]);
        criticalChance  = Mathf.RoundToInt(calculatedStats[Stat.CriticalChance]);
        criticalPercent = Mathf.RoundToInt(calculatedStats[Stat.CriticalPercent]);
        attackRange     = calculatedStats[Stat.Range];

    }

    public void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;
        SetupStats();
    }

    public int GetRecyclePrice()
    {
        return WeaponStatsCalculator.GetRecyclePrice(WeaponData,Level);
    }

    public void Upgrade() => UpgradeTo(Level + 1);
}
