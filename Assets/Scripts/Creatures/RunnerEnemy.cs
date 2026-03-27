/*using UnityEngine;

public class RunnerEnemy : Enemy
{
    protected override void Tick()
    {
        if (HasAttacked || IsDead || IsFinishing || VehicleTransform == null)
            return;

        if (!IsCloseToVehicle(Config.TriggerDistanceToVehicle))
        {
            if (enemyAnimator != null)
                enemyAnimator.PlayIdle();

            return;
        }

        if (!IsCloseToVehicle(Config.AttackDistance))
        {
            if (enemyAnimator != null)
                enemyAnimator.PlayRun();

            MoveTowardsVehicle(Config.MoveSpeed);
            return;
        }

        TriggerAttack();
    }
}*/

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