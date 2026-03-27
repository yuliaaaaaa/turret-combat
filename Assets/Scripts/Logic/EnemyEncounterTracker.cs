using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEncounterTracker : MonoBehaviour
{
    private readonly HashSet<Enemy> _activeEnemies = new();

    public bool HasActiveEnemies => _activeEnemies.Count > 0;

    public event Action EncounterStarted;
    public event Action EncounterEnded;

    public void Register(Enemy enemy)
    {
        if (enemy == null)
            return;

        bool wasEmpty = _activeEnemies.Count == 0;

        if (_activeEnemies.Add(enemy))
        {
            enemy.Removed += OnEnemyRemoved;

            if (wasEmpty)
                EncounterStarted?.Invoke();
        }
    }

    public void Unregister(Enemy enemy)
    {
        if (enemy == null)
            return;

        if (_activeEnemies.Remove(enemy))
        {
            enemy.Removed -= OnEnemyRemoved;

            if (_activeEnemies.Count == 0)
                EncounterEnded?.Invoke();
        }
    }

    public void ClearAll()
    {
        if (_activeEnemies.Count == 0)
            return;

        foreach (Enemy enemy in _activeEnemies)
        {
            if (enemy != null)
                enemy.Removed -= OnEnemyRemoved;
        }

        _activeEnemies.Clear();
        EncounterEnded?.Invoke();
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        Unregister(enemy);
    }
}