using System;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDeathAble deathAble = other.GetComponent<IDeathAble>();
        if (deathAble != null)
        {
            deathAble.Die();
        }
    }
}
