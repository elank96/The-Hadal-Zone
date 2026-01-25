using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private enum AttackPhase
    {
        Locking,
        Holding,
        Lunging,
        Cooldown
    }

    private AttackPhase phase;
    private float phaseTimer;
    private Vector3 lockedDirection;
    private bool hasDealtDamage;
    private Collider targetCollider;
    private Collider selfCollider;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        phase = AttackPhase.Locking;
        phaseTimer = stateMachine.AttackLockDuration;
        lockedDirection = Vector3.up;
        hasDealtDamage = false;
        targetCollider = stateMachine.Target != null ? stateMachine.Target.GetComponent<Collider>() : null;
        selfCollider = stateMachine.GetComponent<Collider>();

        if (stateMachine.AIPath != null)
        {
            stateMachine.AIPath.enabled = false;
        }
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.Target == null)
        {
            return;
        }

        if (phase != AttackPhase.Lunging &&
            (!stateMachine.HasLineOfSight() || stateMachine.DistanceToTarget2D() > stateMachine.AttackRange))
        {
            stateMachine.SwitchState(new EnemyPathfindingState(stateMachine));
            return;
        }

        switch (phase)
        {
            case AttackPhase.Locking:
                UpdateLockedDirection();
                RotateTowards(lockedDirection);
                phaseTimer -= deltaTime;
                if (phaseTimer <= 0f)
                {
                    phase = AttackPhase.Holding;
                    phaseTimer = stateMachine.AttackHoldDuration;
                }
                break;
            case AttackPhase.Holding:
                RotateTowards(lockedDirection);
                phaseTimer -= deltaTime;
                if (phaseTimer <= 0f)
                {
                    phase = AttackPhase.Lunging;
                    phaseTimer = stateMachine.AttackLungeDuration;
                    hasDealtDamage = false;
                }
                break;
            case AttackPhase.Lunging:
                TryDealDamage();
                phaseTimer -= deltaTime;
                if (phaseTimer <= 0f)
                {
                    phase = AttackPhase.Cooldown;
                    phaseTimer = stateMachine.AttackCooldown;
                }
                break;
            case AttackPhase.Cooldown:
                phaseTimer -= deltaTime;
                if (phaseTimer <= 0f)
                {
                    phase = AttackPhase.Locking;
                    phaseTimer = stateMachine.AttackLockDuration;
                }
                break;
        }
    }

    public override void Exit()
    {
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
        if (phase != AttackPhase.Lunging)
        {
            return;
        }

        Vector3 moveDelta = lockedDirection * (stateMachine.AttackLungeSpeed * fixedDeltaTime);
        if (stateMachine.Rigidbody != null)
        {
            Vector3 currentPos = stateMachine.Rigidbody.position;
            stateMachine.Rigidbody.MovePosition(new Vector3(currentPos.x + moveDelta.x, currentPos.y + moveDelta.y, currentPos.z));
        }
        else
        {
            Vector3 currentPos = stateMachine.transform.position;
            stateMachine.transform.position = new Vector3(currentPos.x + moveDelta.x, currentPos.y + moveDelta.y, currentPos.z);
        }
    }

    private void UpdateLockedDirection()
    {
        if (stateMachine.Target == null)
        {
            return;
        }

        Vector3 delta = stateMachine.Target.position - stateMachine.transform.position;
        delta.z = 0f;
        if (delta.sqrMagnitude > 0.0001f)
        {
            lockedDirection = delta.normalized;
        }
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0.0001f)
        {
            return;
        }

        stateMachine.transform.up = direction;
    }

    private void TryDealDamage()
    {
        if (hasDealtDamage || targetCollider == null || selfCollider == null)
        {
            return;
        }

        if (!selfCollider.bounds.Intersects(targetCollider.bounds))
        {
            return;
        }

        hasDealtDamage = true;
        stateMachine.TryApplyDamageToTarget(stateMachine.AttackDamage);
        Debug.Log("Enemy attack hit.");
        stateMachine.PerformAttack();
    }
}
