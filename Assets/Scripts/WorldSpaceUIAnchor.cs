using TMPro;
using UnityEngine;

public class WorldSpaceUIAnchor : MonoBehaviour
{
    public GameObject uiScannablePrefab;
    public GameObject uiScanPrefab;
    private GameObject spawnedScanUI;
    private GameObject spawnedScannableUI;

    [SerializeField] private float canvasOffset = 1f;
    [SerializeField] private float canvasSize = .001f;
    
    private bool isDisplayed = false;

    public void CreateScannableUI()
    {
        spawnedScannableUI = Instantiate(uiScannablePrefab, transform.position, Quaternion.identity);
        spawnedScannableUI.transform.SetParent(this.transform);
        spawnedScannableUI.transform.localPosition = new Vector3(0, 0, -.5f); // Offset above object
        spawnedScannableUI.transform.localScale = new Vector3(.001f, .001f, .001f);
        
        Invoke(nameof(DestroyScannableUI), .99f);
    }
    public void CreateScanUI(ScannableData data)
    {
        if (!isDisplayed)
        {
            spawnedScanUI = Instantiate(uiScanPrefab, transform.position, Quaternion.identity);
            spawnedScanUI.transform.SetParent(this.transform);
            spawnedScanUI.transform.localPosition = new Vector3(0, canvasOffset, -canvasOffset); // Offset above object
            spawnedScanUI.transform.localScale = new Vector3(canvasSize, canvasSize, canvasSize);

            var bridge = spawnedScanUI.GetComponent<ScannableUIBridge>();

            if (bridge != null)
            {
                bridge.speciesText.text = data.objectName;
                bridge.descriptionText.text = data.description;
                bridge.endangeredText.text = data.isEndangered ? "Endangered" : "Not Endangered";
            }

            isDisplayed = true;

            Invoke(nameof(DestroyScanUI), 5f);
        }
    }

    public void DestroyScannableUI()
    {
        isDisplayed = false;
        Destroy(spawnedScannableUI);
    }
    public void DestroyScanUI()
    {
        isDisplayed = false;
        Destroy(spawnedScanUI);
    }
}