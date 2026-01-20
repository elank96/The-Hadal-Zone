using TMPro;
using UnityEngine;

public class WorldSpaceUIAnchor : MonoBehaviour
{
    public GameObject uiPrefab; // Drag your Canvas Prefab here
    private GameObject spawnedUI;

    [SerializeField] private float canvasOffset = 1.5f;
    [SerializeField] private float canvasSize = .001f;
    
    private bool isDisplayed = false;

    public void CreateUI(ScannableData data)
    {
        if (!isDisplayed)
        {
            spawnedUI = Instantiate(uiPrefab, transform.position, Quaternion.identity);
            spawnedUI.transform.SetParent(this.transform);
            spawnedUI.transform.localPosition = new Vector3(0, canvasOffset, -canvasOffset); // Offset above object
            spawnedUI.transform.localScale = new Vector3(canvasSize, canvasSize, canvasSize);

            var bridge = spawnedUI.GetComponent<ScannableUIBridge>();

            if (bridge != null)
            {
                bridge.speciesText.text = data.objectName;
                bridge.descriptionText.text = data.description;
                bridge.endangeredText.text = data.isEndangered ? "Endangered" : "Not Endangered";
            }

            isDisplayed = true;

            Invoke(nameof(DestroyUI), 5f);
        }
    }

    public void DestroyUI()
    {
        isDisplayed = false;
        Destroy(spawnedUI);
    }
}