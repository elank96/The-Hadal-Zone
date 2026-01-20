using UnityEngine;
using UnityEngine.Events;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public PathFollower PathFollower { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackCooldown { get; private set; } = 1f;
    [field: SerializeField] public float StunDuration { get; private set; } = 2f;
    [field: SerializeField] public ParticleSystem StunParticles { get; private set; }

    [Header("Events")]
    [SerializeField] private UnityEvent onAttack;

    private void Awake()
    {
        if (PathFollower == null)
        {
            PathFollower = GetComponent<PathFollower>();
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
