using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DropManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Candy candyPrefab;
    [SerializeField] private Cash cashPrefab;
    [SerializeField] private Chest chestPrefab;

    [Header(" Settings ")]
    [SerializeField] [Range(0,100)] private int cashDropChance;
    [SerializeField] [Range(0,100)] private int chestDropChance;

    [Header(" Pooling ")]
    private ObjectPool<Candy> candyPool;
    private ObjectPool<Cash> cashPool;

    private void Awake()
    {
        Enemy.OnDeath += EnemyDeathCallback;
        Enemy.OnBossDeath += BossEnemyDeathCallback;

        Candy.OnCollected += ReleaseCandy;
        Cash.OnCollected += ReleaseCash;
    }

    private void OnDestroy()
    {
        Enemy.OnDeath -= EnemyDeathCallback;
        Enemy.OnBossDeath -= BossEnemyDeathCallback;

        Candy.OnCollected -= ReleaseCandy;
        Cash.OnCollected -= ReleaseCash;
    }

    private void Start()
    {
        candyPool = new ObjectPool<Candy>(
            CandyCreateFunc, 
            CandyActionOnGet, 
            CandyActionOnRelease,
            CandyActionOnDestroy);
        
        cashPool = new ObjectPool<Cash>(
            CashCreateFunc, 
            CashActionOnGet, 
            CashActionOnRelease,
            CashActionOnDestroy);
    }

    #region Candy Pool
    private Candy CandyCreateFunc()
    {
        Candy newCandy = Instantiate(candyPrefab, transform);
      
        return newCandy;
    }

    private void CandyActionOnGet(Candy candy) => candy.gameObject.SetActive(true);
    private void CandyActionOnRelease(Candy candy) => candy.gameObject.SetActive(false);
    private void CandyActionOnDestroy(Candy candy) => Destroy(candy.gameObject);

    #endregion

    #region Cash Pool
    private Cash CashCreateFunc()
    {
        Cash newCash = Instantiate(cashPrefab, transform);
        
        return newCash;
    }

    private void CashActionOnGet(Cash cash) => cash.gameObject.SetActive(true);
    private void CashActionOnRelease(Cash cash) => cash.gameObject.SetActive(false);
    private void CashActionOnDestroy(Cash cash) => Destroy(cash.gameObject);

    #endregion

    private void EnemyDeathCallback(Vector2 enemyPos)
    {
        bool canDropCash = Random.Range(0, 100) < cashDropChance;
        DroppableCurrency items = canDropCash ? cashPool.Get() : candyPool.Get();
        
        items.transform.position = enemyPos;

        TryDropChest(enemyPos);
    }

    private void BossEnemyDeathCallback(Vector2 enemyPos)
    {
        DropChest(enemyPos);
    }

    private void TryDropChest(Vector2 spawnPos)
    {
        bool canDropChest = Random.Range(0, 100) < chestDropChance;

        if (!canDropChest)
            return;

        DropChest(spawnPos);
    }

    private void DropChest(Vector2 spawnPos)
    {
        Instantiate(chestPrefab, spawnPos, Quaternion.identity, transform);
    }

    private void ReleaseCandy(Candy candy) => candyPool.Release(candy);
    private void ReleaseCash(Cash cash) => cashPool.Release(cash);
}
