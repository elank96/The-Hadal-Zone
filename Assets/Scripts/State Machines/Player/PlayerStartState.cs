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

