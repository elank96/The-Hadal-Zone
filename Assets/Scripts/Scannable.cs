using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField] private ScannableData data;

    public ScannableData ScanObject()
    {
        return data;
    }

    public void DisplayData()
    {
        Debug.Log(data.objectName);
        Debug.Log(data.description);
        Debug.Log(data.isEndangered);
    }

}
