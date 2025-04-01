using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour, ICollectable
{
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    public void Collect(Player player)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveToPlayer(player));
    }

    private IEnumerator MoveToPlayer(Player player)
    {
        float timer = 0;
        Vector3 initializePos = transform.position;

        while (timer < 1)
        {
            transform.position = Vector2.Lerp(initializePos, player.transform.position, timer);
            timer += Time.deltaTime;

            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
}
