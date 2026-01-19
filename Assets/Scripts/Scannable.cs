using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField] private ScannableData data;
    [field: SerializeField] public WorldSpaceUIAnchor WorldSpaceUIAnchor { get; private set; }

    public ScannableData ScanObject()
    {
        return data;
    }

    public void DisplayData()
    {
        WorldSpaceUIAnchor.CreateUI(data);
    }

}
