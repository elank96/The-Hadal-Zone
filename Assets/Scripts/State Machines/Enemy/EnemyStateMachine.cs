using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public AIPath AIPath { get; private set; }
    [field: SerializeField] public AIDestinationSetter DestinationSetter { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;
    [field: SerializeField] public float AttackLockDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float AttackHoldDuration { get; private set; } = 0.5f;
    [field: SerializeField] public float AttackLungeDuration { get; private set; } = 0.2f;
    [field: SerializeField] public float AttackLungeSpeed { get; private set; } = 8f;
    [field: SerializeField] public float AttackDamage { get; private set; } = 10f;
    [field: SerializeField] public float StunDuration { get; private set; } = 2f;
    [field: SerializeField] public ParticleSystem StunParticles { get; private set; }
    [SerializeField] private LayerMask obstacleMask;

    [Header("Events")]
    [SerializeField] private UnityEvent onAttack;

    private void Awake()
    {
        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        if (AIPath == null)
        {
            AIPath = GetComponent<AIPath>();
        }

        if (DestinationSetter == null)
        {
            DestinationSetter = GetComponent<AIDestinationSetter>();
        }
    }

    private void Start()
    {
        SwitchState(new EnemyPathfindingState(this));
    }

    public void PerformAttack()
    {
        onAttack?.Invoke();
    }

    public void ApplyStun()
    {
        SwitchState(new EnemyStunState(this, StunDuration));
    }

    public bool TryApplyDamageToTarget(float damage)
    {
        if (Target == null)
        {
            return false;
        }

        Damageable damageable = Target.GetComponent<Damageable>();
        if (damageable == null)
        {
            return false;
        }

        damageable.ApplyDamage(damage);
        return true;
    }

    public float DistanceToTarget2D()
    {
        if (Target == null)
        {
            return float.PositiveInfinity;
        }

        Vector3 delta = Target.position - transform.position;
        return new Vector2(delta.x, delta.y).magnitude;
    }

    public bool HasLineOfSight()
    {
        if (Target == null)
        {
            return false;
        }

        Vector3 start = transform.position;
        Vector3 end = Target.position;
        start.z = transform.position.z;
        end.z = transform.position.z;

        if (obstacleMask == 0)
        {
            return true;
        }

        bool blocked = Physics.Linecast(start, end, obstacleMask, QueryTriggerInteraction.Ignore);
        Debug.DrawLine(start, end, blocked ? Color.red : Color.green, 0f, false);
        return !blocked;
    }
}
