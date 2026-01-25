using System;
using UnityEngine;

public class PlayerScannerState : PlayerDefaultState
{
    
    float scanCooldown = 0f;
    
    public PlayerScannerState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }
    
    public PlayerScannerState(PlayerStateMachine stateMachine, Quaternion handoffTargetRotation)
        : base(stateMachine, handoffTargetRotation) { }
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.InputReader.UseToolEvent += HandleUseToolEvent;
        Cursor.SetCursor(stateMachine.ScannerCursor, stateMachine.CursorHotspot, CursorMode.Auto);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        scanCooldown -= deltaTime;
        if (scanCooldown < 0f)
        {
            scanCooldown = 1f;
            float scanRadius = 15f;
            Collider[] hitColliders = Physics.OverlapSphere(stateMachine.transform.position, scanRadius);

            foreach (var hit in hitColliders)
            {
                if (hit.TryGetComponent<Scannable>(out var target))
                {
                    target.DisplayScanUIElement();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.UseToolEvent -= HandleUseToolEvent;
    }

    protected override void HandleSwitchToolEvent()
    {
        stateMachine.SwitchState(new PlayerTaserState(this.stateMachine));
    }
    
    private void HandleUseToolEvent()
    {
        Vector2 mousePos = stateMachine.InputReader.InputPosition;
        Ray ray = stateMachine.MainCamera.ScreenPointToRay(mousePos);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<Scannable>(out Scannable target))
            {
                target.DisplayData();
            }
        }
    }
}