using UnityEngine;

public class RoadChunk : MonoBehaviour
{
    [field: SerializeField] public Transform StartPoint { get; private set; }
    [field: SerializeField] public Transform EndPoint { get; private set; }
    [field: SerializeField] public ChunkEnemySpawnGroup EnemySpawnGroup { get; private set; }

    [field: SerializeField] public Transform FinishPoint { get; private set; } 
}