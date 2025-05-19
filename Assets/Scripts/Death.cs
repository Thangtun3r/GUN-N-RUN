using System;
using UnityEngine;

public class Death : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDeathAble deathAble = other.gameObject.GetComponent<IDeathAble>();
        if (deathAble != null)
        {
            deathAble.Die();
        }
    }
}
