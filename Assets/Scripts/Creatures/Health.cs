using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int Current { get; private set; }
    public int Max { get; private set; }

    public event Action<int, int> Changed;
    public event Action Died;

    public void Initialize(int maxHealth)
    {
        Max = Mathf.Max(1, maxHealth);
        Current = Max;
        Changed?.Invoke(Current, Max);
    }

    public void TakeDamage(int damage)
    {
        if (Current <= 0)
            return;

        Current = Mathf.Max(0, Current - Mathf.Max(0, damage));
        Changed?.Invoke(Current, Max);

        if (Current == 0)
            Died?.Invoke();
    }
}