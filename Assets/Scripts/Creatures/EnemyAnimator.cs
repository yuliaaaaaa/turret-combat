/*using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Hit = Animator.StringToHash("Hit");

    [SerializeField] private Animator animator;

    public void PlayIdle()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, false);
    }

    public void PlayRun()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, true);
    }

    public void PlayAttack()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, false);
        animator.SetTrigger(Attack);
    }

    public void PlayFall()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, false);
        animator.SetTrigger(Fall);
    }

    public void PlayHit()
    {
        if (animator == null)
            return;

        animator.SetTrigger(Hit);
    }
}*/
using UnityEngine;

public enum EnemyVisualState
{
    Idle = 0,
    Run = 1
}

public class EnemyAnimator : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Fall = Animator.StringToHash("Fall");
    private static readonly int Hit = Animator.StringToHash("Hit");

    [SerializeField] private Animator animator;

    private EnemyVisualState _currentState = EnemyVisualState.Idle;
    private bool _initialized;

    public void SetState(EnemyVisualState state)
    {
        if (animator == null)
            return;

        if (_initialized && _currentState == state)
            return;

        _initialized = true;
        _currentState = state;

        animator.SetBool(IsRunning, state == EnemyVisualState.Run);
    }

    public void PlayAttack()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, false);
        animator.SetTrigger(Attack);
    }

    public void PlayFall()
    {
        if (animator == null)
            return;

        animator.SetBool(IsRunning, false);
        animator.SetTrigger(Fall);
    }

    public void PlayHit()
    {
        if (animator == null)
            return;

        animator.SetTrigger(Hit);
    }

    public void ResetToIdle()
    {
        _initialized = false;
        SetState(EnemyVisualState.Idle);
    }
}