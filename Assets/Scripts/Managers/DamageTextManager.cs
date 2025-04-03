using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private DamageText damageTextPrefab;

    [Header(" Pooling ")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.OnDamageTaken += EnemyHitCallback;
    }

    private void OnDestroy()
    {
        Enemy.OnDamageTaken -= EnemyHitCallback;
    }

    // Start is called before the first frame update
    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunc, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private DamageText CreateFunc()
    {
        return Instantiate(damageTextPrefab, transform);
    }

    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true);
    }

    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    public void EnemyHitCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        DamageText newDamageText = damageTextPool.Get();

        Vector3 spawnPos = enemyPos + Vector2.up * 1.5f;
        newDamageText.transform.position = spawnPos;

        newDamageText.Animate(damage, isCriticalHit);

        StartCoroutine(IEReleaseObj(newDamageText));
    }

    private IEnumerator IEReleaseObj(DamageText damageText)
    {
        yield return new WaitForSeconds(1f);

        damageTextPool.Release(damageText);
    }


}
