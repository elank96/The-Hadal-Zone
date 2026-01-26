using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }
    public void DamagePlayer()
    {
        if (PlayerStateMachine != null)
        {
            PlayerStateMachine.TakeDamage();
        }
    }
}