using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, ICollectable
{
    public void Collect(Player player)
    {
        Destroy(gameObject);
    }
}
