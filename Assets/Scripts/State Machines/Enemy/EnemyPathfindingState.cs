using UnityEngine;

public class EnemyPathfindingState : EnemyBaseState
{
    public EnemyPathfindingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.PathFollower != null && stateMachine.Target != null)
        {
            stateMachine.PathFollower.enabled = true;
            stateMachine.PathFollower.SetTarget(stateMachine.Target);
        }
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Target == null)
        {
            return;
        }

        if (stateMachine.DistanceToTarget2D() <= stateMachine.AttackRange)
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        }
    }

    public override void Exit()
    {
        if (stateMachine.PathFollower != null)
        {
            stateMachine.PathFollower.enabled = false;
        }
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
    }

    
}
