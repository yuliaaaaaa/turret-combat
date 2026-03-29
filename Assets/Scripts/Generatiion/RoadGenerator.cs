using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private RoadChunk roadChunkPrefab;
    [SerializeField] private RoadChunk finalChunkPrefab;

    [Header("References")]
    [SerializeField] private Transform carTransform;

    [Header("Generation")]
    [SerializeField] private int initialChunkCount = 6;
    [SerializeField] private float spawnTriggerDistance = 30f;
    [SerializeField] private float levelLength = 300f;

    private readonly Queue<RoadChunk> _activeChunks = new();

    private EnemySpawner _enemySpawner;
    private GameStateService _gameStateService;
    private LevelProgressService _progressService;

    private RoadChunk _lastChunk;
    private Transform _finishPoint;
    private float _spawnedLength;
    private bool _finalChunkSpawned;

    public Transform FinishPoint => _finishPoint;

    [Inject]
    public void Construct(
        EnemySpawner enemySpawner,
        GameStateService gameStateService,
        LevelProgressService progressService)
    {
        _enemySpawner = enemySpawner;
        _gameStateService = gameStateService;
        _progressService = progressService;
    }

    private void Start()
    {
        ResetGenerator();
    }

    private void Update()
    {
        if (!CanGenerate())
            return;

        float distanceToEnd = _lastChunk.EndPoint.position.z - carTransform.position.z;

        if (distanceToEnd >= spawnTriggerDistance)
            return;

        bool shouldSpawnFinal = _spawnedLength >= levelLength;

        SpawnNextChunk(shouldSpawnFinal);
        RemoveOldChunks();
    }

    public Transform GetFinishPoint()
    {
        return _finishPoint;
    }

    public void ResetGenerator()
    {
        ClearChunks();

        _spawnedLength = 0f;
        _finalChunkSpawned = false;
        _lastChunk = null;
        _finishPoint = null;

        for (int i = 0; i < initialChunkCount; i++)
        {
            bool shouldSpawnFinal = _spawnedLength >= levelLength;

            SpawnNextChunk(shouldSpawnFinal);

            if (_finalChunkSpawned)
                break;
        }

        if (_progressService != null && carTransform != null)
            _progressService.Initialize(carTransform.position.z, levelLength);
    }

    private bool CanGenerate()
    {
        if (_gameStateService == null || _gameStateService.CurrentState != GameState.Playing)
            return false;

        if (_lastChunk == null || carTransform == null)
            return false;

        if (_finalChunkSpawned)
            return false;

        return true;
    }

    private void SpawnNextChunk(bool spawnFinal)
    {
        RoadChunk prefab = spawnFinal ? finalChunkPrefab : roadChunkPrefab;
        if (prefab == null)
            return;

        RoadChunk newChunk = InstantiateChunk(prefab);

        _activeChunks.Enqueue(newChunk);
        _lastChunk = newChunk;

        _spawnedLength += GetChunkLength(newChunk);

        if (spawnFinal)
        {
            _finalChunkSpawned = true;

            _finishPoint = newChunk.FinishPoint != null
                ? newChunk.FinishPoint
                : newChunk.EndPoint;

            return;
        }

        _enemySpawner?.SpawnForChunk(newChunk);
    }

    private RoadChunk InstantiateChunk(RoadChunk prefab)
    {
        if (_lastChunk == null)
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);

        Vector3 spawnPosition = _lastChunk.EndPoint.position - prefab.StartPoint.localPosition;
        return Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

    private float GetChunkLength(RoadChunk chunk)
    {
        if (chunk == null || chunk.StartPoint == null || chunk.EndPoint == null)
            return 0f;

        return Mathf.Abs(chunk.EndPoint.position.z - chunk.StartPoint.position.z);
    }

    private void RemoveOldChunks()
    {
        while (_activeChunks.Count > initialChunkCount) 
        {
            RoadChunk oldChunk = _activeChunks.Dequeue();

            if (oldChunk == null)
                continue;

            _enemySpawner?.DespawnForChunk(oldChunk);
            Destroy(oldChunk.gameObject);
        }
    }

    private void ClearChunks()
    {
        while (_activeChunks.Count > 0)
        {
            RoadChunk chunk = _activeChunks.Dequeue();

            if (chunk == null)
                continue;

            _enemySpawner?.DespawnForChunk(chunk);
            Destroy(chunk.gameObject);
        }
    }
}