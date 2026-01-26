using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }        // field: allows us to serialize a property, property is configured such that anyone can get it but you cant set it
    [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
    [field: SerializeField] public float TopSpeed { get; private set; }
    [field: SerializeField] public float Acceleration { get; private set; }
    [field: SerializeField] public float RotateSpeed { get; private set; }
    [field: SerializeField] public float RotateDeadZone { get; private set; }
    [field: SerializeField] public float PropellerSpinSpeed { get; private set; }
    [field: SerializeField] public ParticleSystem ForwardParticlesL { get; private set; }
    [field: SerializeField] public ParticleSystem ForwardParticlesR { get; private set; }
    [field: SerializeField] public ParticleSystem UpParticlesL { get; private set; }
    [field: SerializeField] public ParticleSystem UpParticlesR { get; private set; }
    [field: SerializeField] public float ParticleAmount { get; private set; }
    [field: SerializeField] public Texture2D DefaultCursor { get; private set; }
    [field: SerializeField] public Texture2D ScannerCursor { get; private set; }
    [field: SerializeField] public Texture2D TaserCursor { get; private set; }
    [field: SerializeField] public Vector2 CursorHotspot { get; private set; } = new Vector2(16, 16);
    [field: SerializeField] public Transform ForwardPropL { get; private set; }
    [field: SerializeField] public Transform ForwardPropR { get; private set; }
    [field: SerializeField] public Transform UpPropL { get; private set; }
    [field: SerializeField] public Transform UpPropR { get; private set; }
    [field: Header("Health & Visuals")]
    [field: SerializeField] public int Health { get; private set; } = 3;
    [field: SerializeField] public Image DamageOverlay { get; private set; } // The "Display" component
    [field: SerializeField] public Texture2D[] DamageTextures { get; private set; } // Put DMG1, 2, 3 here in order
    [field: SerializeField] public bool IsInvincible {get; private set;}
    [field: SerializeField] public Image CooldownUIElement {get; private set;}
    public Camera MainCamera { get; private set; }
    private void Start()
    {
        MainCamera = Camera.main;
        SwitchState(new PlayerDefaultState(this));
    }

    public void TakeDamage()
    {
        if (IsInvincible)
        {
            return;
        }
        
        --Health;
        IsInvincible = true;
        Invoke(nameof(AllowDamage),1f);
        
        if (DamageOverlay == null || DamageTextures == null) return;
        
        DamageOverlay.gameObject.SetActive(true);
        
        int textureIndex = DamageTextures.Length - Health - 1;

        if (textureIndex >= 0 && textureIndex < DamageTextures.Length)
        {
            // Convert Texture2D to Sprite for the UI Image component
            Texture2D tex = DamageTextures[textureIndex];
            DamageOverlay.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            
            // Ensure the overlay is visible
            DamageOverlay.enabled = true;
        }
        
    }

    private void AllowDamage()
    {
        IsInvincible = false;
    }
}