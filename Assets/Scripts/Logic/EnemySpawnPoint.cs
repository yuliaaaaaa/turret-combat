using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [field: SerializeField] public EnemyType EnemyType { get; private set; }
}