using UnityEngine;
using TMPro;

public class ScanPopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    private Transform targetTransform;
    private Camera mainCamera;
    private RectTransform rectTransform;

    public void Setup(ScannableData data, Transform target)
    {
        nameText.text = data.objectName;
        descriptionText.text = data.description;
        targetTransform = target;
        mainCamera = Camera.main;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (targetTransform == null) return;

        // 1. Convert World Position to Screen Position
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetTransform.position);

        // 2. Check if object is behind the camera or out of bounds
        // screenPos.z is the distance from the camera. If negative, it's behind.
        if (screenPos.z < 0 || 
            screenPos.x < 0 || screenPos.x > Screen.width || 
            screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Destroy(gameObject);
            return;
        }

        // 3. Update UI position
        rectTransform.position = screenPos;
    }
}