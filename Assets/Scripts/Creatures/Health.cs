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
        Max = maxHealth;
        Current = maxHealth;
        Changed?.Invoke(Current, Max);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0 || Current <= 0)
            return;

        Current -= damage;

        if (Current < 0)
            Current = 0;

        Changed?.Invoke(Current, Max);

        if (Current == 0)
            Died?.Invoke();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || Current <= 0)
            return;

        Current += amount;

        if (Current > Max)
            Current = Max;

        Changed?.Invoke(Current, Max);
    }
}