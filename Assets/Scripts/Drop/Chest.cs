using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    public static Action OnCollected;

    public void Collect(Player player)
    {
        OnCollected?.Invoke();
        Destroy(gameObject);
    }
}
