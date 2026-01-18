using System;
using UnityEngine;

public class PlayerTaserState : PlayerDefaultState
{
    public PlayerTaserState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }
    
    public PlayerTaserState(PlayerStateMachine stateMachine, Quaternion handoffTargetRotation)
        : base(stateMachine, handoffTargetRotation) { }
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.InputReader.UseToolEvent += HandleUseToolEvent;
        Cursor.SetCursor(stateMachine.TaserCursor, stateMachine.CursorHotspot, CursorMode.Auto);
        Cursor.visible = true;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.UseToolEvent -= HandleUseToolEvent;
        Cursor.visible = false;
    }

    protected override void HandleSwitchToolEvent()
    {
        stateMachine.SwitchState(new PlayerDefaultState(this.stateMachine));
    }
    
    private void HandleUseToolEvent()
    {
        throw new NotImplementedException("HandleSwitchToolEvent");
    }
}