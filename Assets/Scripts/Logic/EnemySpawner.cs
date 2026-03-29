using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyBinding
    {
        public EnemyType type;
        public Enemy prefab;
        public EnemyConfig config;
    }

    [SerializeField] private List<EnemyBinding> enemyBindings = new();

    private readonly Dictionary<RoadChunk, List<Enemy>> _chunkEnemies = new();
    private readonly Dictionary<EnemyType, EnemyBinding> _bindingsByType = new();

    private DiContainer _container;
    private EnemyEncounterService _enemyEncounterService;

    [Inject]
    public void Construct(DiContainer container, EnemyEncounterService enemyEncounterService)
    {
        _container = container;
        _enemyEncounterService = enemyEncounterService;
    }

    private void Awake()
    {
        BuildBindingsMap();
    }

    public void SpawnForChunk(RoadChunk chunk)
    {
        if (!CanSpawnForChunk(chunk))
            return;

        IReadOnlyList<EnemySpawnPoint> spawnPoints = chunk.EnemySpawnGroup.SpawnPoints;
        List<Enemy> spawnedEnemies = new(spawnPoints.Count);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Enemy enemy = Spawn(spawnPoints[i]);
            if (enemy == null)
                continue;

            spawnedEnemies.Add(enemy);
        }

        _chunkEnemies.Add(chunk, spawnedEnemies);
    }

    public void DespawnForChunk(RoadChunk chunk)
    {
        if (chunk == null)
            return;

        if (!_chunkEnemies.TryGetValue(chunk, out List<Enemy> enemies))
            return;

        DespawnEnemies(enemies);
        _chunkEnemies.Remove(chunk);
    }

    public void ClearAll()
    {
        foreach (KeyValuePair<RoadChunk, List<Enemy>> pair in _chunkEnemies)
            DespawnEnemies(pair.Value);

        _chunkEnemies.Clear();
        _enemyEncounterService?.Clear();
    }

    private bool CanSpawnForChunk(RoadChunk chunk)
    {
        if (chunk == null || chunk.EnemySpawnGroup == null)
            return false;

        if (_chunkEnemies.ContainsKey(chunk))
            return false;

        return true;
    }

    private Enemy Spawn(EnemySpawnPoint spawnPoint)
    {
        if (spawnPoint == null)
            return null;

        if (!_bindingsByType.TryGetValue(spawnPoint.EnemyType, out EnemyBinding binding))
        {
            Debug.LogError($"No binding configured for enemy type: {spawnPoint.EnemyType}", this);
            return null;
        }

        if (binding.prefab == null)
        {
            Debug.LogError($"Enemy prefab is missing for enemy type: {spawnPoint.EnemyType}", this);
            return null;
        }

        if (binding.config == null)
        {
            Debug.LogError($"Enemy config is missing for enemy type: {spawnPoint.EnemyType}", this);
            return null;
        }

        Quaternion spawnRotation = spawnPoint.transform.rotation * Quaternion.Euler(0f, 180f, 0f);

        Enemy enemy = _container.InstantiatePrefabForComponent<Enemy>(
            binding.prefab.gameObject,
            spawnPoint.transform.position,
            spawnRotation,
            null);

        enemy.Initialize(binding.config);
        enemy.Removed += OnEnemyRemoved;

        _enemyEncounterService?.Register(enemy);

        return enemy;
    }

    private void DespawnEnemies(List<Enemy> enemies)
    {
        if (enemies == null)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = enemies[i];
            if (enemy == null)
                continue;

            enemy.Removed -= OnEnemyRemoved;

            if (enemy.CanBeForceDespawned)
                enemy.ForceDespawnImmediate();
        }
    }

    private void OnEnemyRemoved(Enemy removedEnemy)
    {
        if (removedEnemy == null)
            return;

        removedEnemy.Removed -= OnEnemyRemoved;
        _enemyEncounterService?.Unregister(removedEnemy);

        RoadChunk owningChunk = null;

        foreach (KeyValuePair<RoadChunk, List<Enemy>> pair in _chunkEnemies)
        {
            if (!pair.Value.Remove(removedEnemy))
                continue;

            if (pair.Value.Count == 0)
                owningChunk = pair.Key;

            break;
        }

        if (owningChunk != null)
            _chunkEnemies.Remove(owningChunk);
    }

    private void BuildBindingsMap()
    {
        _bindingsByType.Clear();

        for (int i = 0; i < enemyBindings.Count; i++)
        {
            EnemyBinding binding = enemyBindings[i];
            if (binding == null)
                continue;

            if (_bindingsByType.ContainsKey(binding.type))
            {
                Debug.LogWarning($"Duplicate enemy binding for type: {binding.type}", this);
                continue;
            }

            _bindingsByType.Add(binding.type, binding);
        }
    }
}