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
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.UseToolEvent -= HandleUseToolEvent;
    }

    protected override void HandleSwitchToolEvent()
    {
        stateMachine.SwitchState(new PlayerDefaultState(this.stateMachine));
    }
    
    private void HandleUseToolEvent()
    {
        Ray ray = stateMachine.MainCamera.ScreenPointToRay(stateMachine.InputReader.InputPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<Scannable>(out Scannable target))
            {
                if (target.hasBeenScanned && !target.GetData().isEndangered)
                {
                    ExecuteShock(target.transform.position);
                }
                else
                {
                    Debug.Log("Shock Failed!");
                }

            }
        }
    }
    
    private void ExecuteShock(Vector3 origin)
    {
        float shockRadius = 3f; 
        Collider[] hitColliders = Physics.OverlapSphere(origin, shockRadius);

        foreach (var hit in hitColliders)
        {
            // Assuming your fish have a "FishController" or "Stunnable" script
            if (hit.TryGetComponent<Stunnable>(out var target))
            {
                target.Stun();
            }
        }
    }
}