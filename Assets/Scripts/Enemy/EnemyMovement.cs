using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header(" Elements ")]
    private Player player;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        int currentWaveIndex = WaveManager.instance.GetCurrentWaveIndex();
        moveSpeed *= (1 + (float)currentWaveIndex / 100);
    }

    // Update is called once per frame
    void Update()
    {
        //if (player != null)
        //    FollowPlayer();
    }

    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    public void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }
}
