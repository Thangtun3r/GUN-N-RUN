using System;
using UnityEngine;

public class LosingEvent : MonoBehaviour
{
    public static event Action onPlayerDeath;

    public static void RaisePlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }
}
