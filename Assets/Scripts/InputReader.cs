using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovementValue { get; private set; }
    public Vector2 InputPosition { get; private set; }
    
    public event Action SwitchToolEvent;
    public event Action UseToolEvent;
    
    private Controls controls;
    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnInputPos(InputAction.CallbackContext context)
    {
        InputPosition = context.ReadValue<Vector2>();
    }

    public void OnSwitchTool(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        SwitchToolEvent?.Invoke();
    }
    
    public void OnUseTool(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }

        UseToolEvent?.Invoke();
    }
}