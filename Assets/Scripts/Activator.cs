using System.Collections.Generic;
using UnityEngine;

public class ObjectActivationTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask triggerLayer;
    [SerializeField] private bool deactivateOnExit = false;

    [Header("Outputs")]
    [SerializeField] private List<GameObject> objectsToActivate = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        // Equivalent to: if ((triggerLayer.value & (1 << other.gameObject.layer)) > 0)
        // Checks if the colliding object's layer is in our allowed mask
        if (IsInLayerMask(other.gameObject, triggerLayer))
        {
            SetObjectsState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (deactivateOnExit && IsInLayerMask(other.gameObject, triggerLayer))
        {
            SetObjectsState(false);
        }
    }

    private void SetObjectsState(bool state)
    {
        foreach (var obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(state);
            }
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask == (mask | (1 << obj.layer)));
    }
}