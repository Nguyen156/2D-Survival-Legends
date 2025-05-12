using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private CircleCollider2D collectableCollider;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();

        WaveManager.OnWaveCompleted += WaveCompletedCallback;
    }

    private void OnDestroy()
    {
        WaveManager.OnWaveCompleted -= WaveCompletedCallback;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICollectable item))
        {
            if(!other.IsTouching(collectableCollider))
                return;

            item.Collect(player);
        }
    }

    private void WaveCompletedCallback()
    {
        collectableCollider.radius = 100;
        Invoke(nameof(ResetDetectionRadius), 1f);
    }

    private void ResetDetectionRadius() => collectableCollider.radius = 1;
}
