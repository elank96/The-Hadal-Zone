using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }        // field: allows us to serialize a property, property is configured such that anyone can get it but you cant set it
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    private void Start()
    {
        SwitchState(new PlayerStartState(this));
    }
}