/*using UnityEngine;

public class AmbushEnemy : Enemy
{
    protected override void Tick()
    {
        if (HasAttacked || IsDead || IsFinishing)
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
                enemyAnimator.PlayIdle();

            return;
        }

        TriggerAttack();
    }
}*/
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