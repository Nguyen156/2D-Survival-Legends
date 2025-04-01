using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : DroppableCurrency
{
    [Header(" Actions ")]
    public static Action<Cash> OnCollected;

    protected override void Collected()
    {
        OnCollected?.Invoke(this);
    }
}
