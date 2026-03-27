using System.Collections.Generic;
using UnityEngine;

public class ChunkEnemySpawnGroup : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnPoint> spawnPoints = new();

    public IReadOnlyList<EnemySpawnPoint> SpawnPoints => spawnPoints;
}