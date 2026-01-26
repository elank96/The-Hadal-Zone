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

        float distance = stateMachine.DistanceToTarget2D();
        bool hasLineOfSight = stateMachine.HasLineOfSight();
        //Debug.Log($"Enemy pathfinding: distance={distance:F2}, range={stateMachine.AttackRange:F2}, LOS={hasLineOfSight}");

        if (distance <= stateMachine.AttackRange && hasLineOfSight)
        {
            Debug.Log("Enemy switching to Attack state.");
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
