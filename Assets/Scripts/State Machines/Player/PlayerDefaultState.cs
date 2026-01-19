using System;
using UnityEngine;

public class PlayerDefaultState : PlayerBaseState
{
    protected Quaternion targetRotation;

    public PlayerDefaultState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
        targetRotation = Quaternion.Euler(0, 0, 0);
    }
    
    public PlayerDefaultState(PlayerStateMachine stateMachine, Quaternion handoffTargetRotation) : base(stateMachine)
    {
        targetRotation = handoffTargetRotation;
    }

    public override void Enter()
    {
        stateMachine.InputReader.SwitchToolEvent += HandleSwitchToolEvent;
        Cursor.SetCursor(stateMachine.DefaultCursor, stateMachine.CursorHotspot, CursorMode.Auto);
    }

    public override void Tick(float deltaTime)
    {
        
        //Rotate Propellers
        float horizontallInput = stateMachine.InputReader.MovementValue.x;
        float rotationAmount = horizontallInput * stateMachine.PropellerSpinSpeed * deltaTime;
        stateMachine.ForwardPropL.Rotate(0, 0, rotationAmount, Space.Self);
        stateMachine.ForwardPropR.Rotate(0, 0, rotationAmount, Space.Self);
    }

    public override void PhysicsTick(float fixedDeltaTime)
    {
        //Handle Movement
        Vector3 input = stateMachine.InputReader.MovementValue;

        Vector3 targetVelocity = new Vector3(input.x, input.y, 0) * stateMachine.TopSpeed;
        Vector3 currentVelocity = stateMachine.Rigidbody.linearVelocity;
        
        Vector3 velocityChange = targetVelocity - currentVelocity;
        
        // If you want gravity to work normally, don't correct the Y velocity 
        // unless the player is actively providing vertical input.
        if (input.y == 0) 
        {
            velocityChange.y = 0;
        }

        stateMachine.Rigidbody.AddForce(velocityChange * stateMachine.Acceleration * fixedDeltaTime, ForceMode.VelocityChange);
        
        //Handle Look Direction
        Vector2 mouseScreenPos = stateMachine.InputReader.InputPosition;
        Vector3 playerScreenPos = stateMachine.MainCamera.WorldToScreenPoint(stateMachine.Rigidbody.position);

        // Determine target rotation
        if (Math.Abs(mouseScreenPos.x - playerScreenPos.x) > stateMachine.RotateDeadZone)
        {
            if (mouseScreenPos.x <= playerScreenPos.x)
            {
                targetRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                targetRotation = Quaternion.Euler(0, 180.1f, 0); //180.1 to guarantee the player rotates around the front
            }
        }

        stateMachine.Rigidbody.transform.rotation = Quaternion.Slerp(
            stateMachine.Rigidbody.transform.rotation, 
            targetRotation, 
            stateMachine.RotateSpeed * fixedDeltaTime
        );
    }

    public override void Exit()
    {
        stateMachine.InputReader.SwitchToolEvent -= HandleSwitchToolEvent;
    }

    protected virtual void HandleSwitchToolEvent()
    {
        stateMachine.SwitchState(new PlayerScannerState(this.stateMachine));
    }

}

