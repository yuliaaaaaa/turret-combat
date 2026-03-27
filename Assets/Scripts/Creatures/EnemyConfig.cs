using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Game/Enemies/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [Header("Stats")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 3;
    [field: SerializeField] public float MoveSpeed { get; private set; } = 3f;
    [field: SerializeField] public int Damage { get; private set; } = 1;

    [Header("Attack")]
    [field: SerializeField] public float AttackDistance { get; private set; } = 1.5f;
    [field: SerializeField] public float AttackDelay { get; private set; } = 0.4f;
    [field: SerializeField] public float TriggerDistanceToVehicle { get; private set; } = 4f;

    [Header("Lifecycle")]
    [field: SerializeField] public float DespawnDelayAfterFall { get; private set; } = 1.5f;
}