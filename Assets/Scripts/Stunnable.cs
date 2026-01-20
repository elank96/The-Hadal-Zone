using UnityEngine;

public class Stunnable : MonoBehaviour
{
    [field: SerializeField] public EnemyStateMachine EnemyStateMachine { get; private set; }

    public void Stun()
    {
        EnemyStateMachine.ApplyStun();
    }
}
