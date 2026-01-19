using System;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }        // field: allows us to serialize a property, property is configured such that anyone can get it but you cant set it
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public float TopSpeed { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float RotateSpeed { get; private set; }
    [field: SerializeField] public float RotateDeadZone { get; private set; }
    [field: SerializeField] public float PropellerSpinSpeed { get; private set; }
    [field: SerializeField] public Texture2D DefaultCursor { get; private set; }
    [field: SerializeField] public Texture2D ScannerCursor { get; private set; }
    [field: SerializeField] public Texture2D TaserCursor { get; private set; }
    [field: SerializeField] public Vector2 CursorHotspot { get; private set; } = new Vector2(16, 16);
    [field: SerializeField] public Transform ForwardPropL { get; private set; }
    [field: SerializeField] public Transform ForwardPropR { get; private set; }
    public Camera MainCamera { get; private set; }
    private void Start()
    {
        MainCamera = Camera.main;
        SwitchState(new PlayerDefaultState(this));
    }
}