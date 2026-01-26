using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private UnityEvent onDamaged;
    [SerializeField] private UnityEvent onDeath;
    [field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }

    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        if (amount <= 0f || CurrentHealth <= 0f)
        {
            return;
        }

        CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
        onDamaged?.Invoke();
        if (PlayerStateMachine != null)
        {
            PlayerStateMachine.TakeDamage();
        }

        if (CurrentHealth <= 0f)
        {
            onDeath?.Invoke();
        }
    }

    public void DamagePlayer()
    {
        if (PlayerStateMachine != null)
        {
            PlayerStateMachine.TakeDamage();
        }
    }
}