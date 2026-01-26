using UnityEngine;

public class Damageable : MonoBehaviour
{
    [field: SerializeField] public PlayerStateMachine PlayerStateMachine { get; private set; }

    public void DamagePlayer()
    {
        PlayerStateMachine.TakeDamage();
    }
}
