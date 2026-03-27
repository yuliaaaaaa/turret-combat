using UnityEngine;

public class RunnerEnemy : Enemy
{
    protected override void Tick(float distanceToVehicle)
    {
        if (distanceToVehicle > Config.TriggerDistanceToVehicle)
        {
            SetAnimationState(EnemyVisualState.Idle);
            return;
        }

        if (distanceToVehicle > Config.AttackDistance)
        {
            SetAnimationState(EnemyVisualState.Run);
            MoveTowardsVehicle(Config.MoveSpeed);
            return;
        }

        TriggerAttack();
    }
}