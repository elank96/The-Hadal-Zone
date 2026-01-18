using System;
using UnityEngine;

public class PlayerStartState : PlayerBaseState
{

    public PlayerStartState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.SwitchToolEvent += HandleSwitchToolEvent;
        stateMachine.InputReader.UseToolEvent += HandleUseToolEvent;
    }

    public override void Tick(float deltaTime)
    {
        //Handle Movement
        Vector3 movementInput = stateMachine.InputReader.MovementValue;
        stateMachine.Rigidbody.AddForce(movementInput * stateMachine.MoveSpeed * deltaTime, ForceMode.VelocityChange);
        Vector2 mouseScreenPos = stateMachine.InputReader.InputPosition;
        float distanceToCamera = Math.Abs(Camera.main.transform.position.z - stateMachine.Rigidbody.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, distanceToCamera));
        if (mouseWorldPos.x <= stateMachine.Rigidbody.position.x)
        {
            stateMachine.Rigidbody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            stateMachine.Rigidbody.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public override void Exit()
    {
        stateMachine.InputReader.SwitchToolEvent -= HandleSwitchToolEvent;
        stateMachine.InputReader.UseToolEvent -= HandleUseToolEvent;
    }

    private void HandleSwitchToolEvent()
    {

    }


    private void HandleUseToolEvent()
    {

    }
}

