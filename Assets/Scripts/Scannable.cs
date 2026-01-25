using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField] private ScannableData data;
    [field: SerializeField] public WorldSpaceUIAnchor WorldSpaceUIAnchor { get; private set; }
    public bool hasBeenScanned = false;

    public ScannableData GetData()
    {
        return data;
    }
    public void DisplayData()
    {
        hasBeenScanned = true;
        WorldSpaceUIAnchor.CreateScanUI(data);
    }

    public void DisplayScanUIElement()
    {
        if (hasBeenScanned == false)
        {
            WorldSpaceUIAnchor.CreateScannableUI();
        }
    }

}
