using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : DroppableCurrency
{
    [Header(" Actions ")]
    public static Action<Candy> OnCollected;

    protected override void Collected()
    {
        OnCollected?.Invoke(this);
    }
}
