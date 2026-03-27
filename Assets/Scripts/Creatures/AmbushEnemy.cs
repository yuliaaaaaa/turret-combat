using UnityEngine;

public class AmbushEnemy : Enemy
{
    protected override void Tick(float distanceToVehicle)
    {
        SetAnimationState(EnemyVisualState.Idle);

        if (distanceToVehicle > Config.TriggerDistanceToVehicle)
            return;

        if (distanceToVehicle > Config.AttackDistance)
            return;

        TriggerAttack();
    }
}