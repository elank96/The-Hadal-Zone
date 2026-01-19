using UnityEngine;

public class EnemyStunState : EnemyBaseState
{
    private float remainingTime;

    public EnemyStunState(EnemyStateMachine stateMachine, float durationSeconds) : base(stateMachine)
    {
        remainingTime = Mathf.Max(0f, durationSeconds);
    }

    public override void Enter()
    {
        if (stateMachine.AIPath != null)
        {
            stateMachine.AIPath.enabled = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        if (remainingTime <= 0f)
        {
            ExitStun();
            return;
        }

        remainingTime -= deltaTime;
        if (remainingTime <= 0f)
        {
            ExitStun();
        }
    }

    public override void Exit()
    {
    }

    private void ExitStun()
    {
        if (stateMachine.DistanceToTarget2D() <= stateMachine.AttackRange)
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        }
        else
        {
            stateMachine.SwitchState(new EnemyPathfindingState(stateMachine));
        }
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
    }
}
