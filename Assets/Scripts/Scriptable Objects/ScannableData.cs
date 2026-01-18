using UnityEngine;

[CreateAssetMenu(fileName = "ScannableData", menuName = "Scriptable Objects/ScannableData")]
public class ScannableData : ScriptableObject
{
    public string objectName;
    [TextArea(3, 10)]
    public string description;
    public bool isEndangered;
}
