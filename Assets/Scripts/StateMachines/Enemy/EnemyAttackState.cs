using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        attackTimer = 0f;
        if (stateMachine.PathFollower != null)
        {
            stateMachine.PathFollower.enabled = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Target == null)
        {
            return;
        }

        if (stateMachine.DistanceToTarget2D() > stateMachine.AttackRange)
        {
            stateMachine.SwitchState(new EnemyPathfindingState(stateMachine));
            return;
        }

        attackTimer -= deltaTime;
        if (attackTimer <= 0f)
        {
            Debug.Log("Enemy attack triggered.");
            stateMachine.PerformAttack();
            attackTimer = stateMachine.AttackCooldown;
        }
    }

    public override void Exit()
    {
    }
}
