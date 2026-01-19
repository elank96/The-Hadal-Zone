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
    [field: SerializeField] public float StunDuration { get; private set; } = 2f;

    [Header("Events")]
    [SerializeField] private UnityEvent onAttack;

    private void Awake()
    {
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

    public float DistanceToTarget2D()
    {
        if (Target == null)
        {
            return float.PositiveInfinity;
        }

        Vector3 delta = Target.position - transform.position;
        return new Vector2(delta.x, delta.y).magnitude;
    }
}
