using UnityEngine;

public class AlwaysLookUp : MonoBehaviour
{
    [Header("Control Settings")]
    [SerializeField] private float smoothTime = 10f; // Hysteresis/Smoothing factor
    
    private float _currentYOffset = 0f;

    void LateUpdate()
    {
        if (transform.parent == null) return;

        // 1. Get the Parent's Z rotation from its local space
        // We use the parent's rotation as the "sensor input"
        float parentZ = transform.parent.localEulerAngles.z;

        // 2. Normalize angle to (-180, 180] 
        // Essential for checking the 0 to -180 range correctly
        if (parentZ > 180) parentZ -= 360;

        // 3. Logic: If leaning left (Z between -180 and 0), flip Y
        float targetY = (parentZ < 0 && parentZ > -180) ? 180f : 0f;

        // 4. Smooth the transition
        // Similar to a first-order lag filter in a control system
        _currentYOffset = Mathf.LerpAngle(_currentYOffset, targetY, Time.deltaTime * smoothTime);

        // 5. Apply to child local rotation
        // We keep local Z and X at 0 so it stays flush with the parent's gimbal
        // but rotates 180 degrees around its own Y axis.
        transform.localRotation = Quaternion.Euler(0, _currentYOffset, -90);
    }
}
