using System;
using UnityEngine;

public class PlayerTaserState : PlayerDefaultState
{
    private float TaseCooldown = 3f;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 10f;
    
    public PlayerTaserState(PlayerStateMachine stateMachine)
        : base(stateMachine) { }
    
    public PlayerTaserState(PlayerStateMachine stateMachine, Quaternion handoffTargetRotation)
        : base(stateMachine, handoffTargetRotation) { }
    
    public override void Enter()
    {
        base.Enter();
        stateMachine.InputReader.UseToolEvent += HandleUseToolEvent;
        Cursor.SetCursor(stateMachine.TaserCursor, stateMachine.CursorHotspot, CursorMode.Auto);
        stateMachine.CooldownUIElement.enabled = true;
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        TaseCooldown -= deltaTime;
        UpdateCooldownDisplay(deltaTime);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.InputReader.UseToolEvent -= HandleUseToolEvent;
        stateMachine.CooldownUIElement.enabled = false;
    }

    protected override void HandleSwitchToolEvent()
    {
        stateMachine.SwitchState(new PlayerDefaultState(this.stateMachine));
    }
    
    private void UpdateCooldownDisplay(float deltaTime)
    {
        float normalizedCooldown = Mathf.Clamp01(TaseCooldown / 5f);

        stateMachine.CooldownUIElement.fillAmount = normalizedCooldown;
        
        Vector2 shakeOffset = Vector2.zero;

        if (shakeDuration > 0)
        {
            // Generate a random offset within a circle
            shakeOffset = UnityEngine.Random.insideUnitCircle * shakeMagnitude;
            shakeDuration -= deltaTime;
        }

        stateMachine.CooldownUIElement.transform.position = stateMachine.InputReader.InputPosition + shakeOffset;

        //stateMachine.CooldownUIElement.color = Color.Lerp(Color.green, Color.red, normalizedCooldown);
    }
    
    private void HandleUseToolEvent()
    {
        if (TaseCooldown > 0f)
        {
            shakeDuration = .2f;
            return;
        }
        Ray ray = stateMachine.MainCamera.ScreenPointToRay(stateMachine.InputReader.InputPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent<Scannable>(out Scannable target))
            {
                if (target.hasBeenScanned && !target.GetData().isEndangered)
                {
                    ExecuteShock(target.transform.position);
                    TaseCooldown = 5f;
                }
                else
                {
                    TaseCooldown = 3f;
                    shakeDuration = .2f;
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
            if (hit.TryGetComponent<Stunnable>(out var target))
            {
                target.Stun();
            }
        }
    }
}