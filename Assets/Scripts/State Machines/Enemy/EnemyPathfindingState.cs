using UnityEngine;

public class EnemyPathfindingState : EnemyBaseState
{
    public EnemyPathfindingState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (stateMachine.AIPath != null)
        {
            stateMachine.AIPath.enabled = true;
        }

        if (stateMachine.DestinationSetter != null && stateMachine.Target != null)
        {
            stateMachine.DestinationSetter.target = stateMachine.Target;
        }
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Target == null)
        {
            return;
        }

        if (stateMachine.DistanceToTarget2D() <= stateMachine.AttackRange && stateMachine.HasLineOfSight())
        {
            stateMachine.SwitchState(new EnemyAttackState(stateMachine));
        }
    }

    public override void Exit()
    {
        if (stateMachine.AIPath != null)
        {
            stateMachine.AIPath.enabled = false;
        }
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
    }

    
}
