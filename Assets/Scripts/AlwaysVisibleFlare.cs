using System;
using TMPro;
using UnityEngine;

public class AlwaysVisibleFlare : MonoBehaviour
{
    public GameObject lensFlarePrefab;
    private GameObject spawnedLensFlare;

    [SerializeField] private float canvasOffset = 1f;
    [SerializeField] private float canvasSize = .001f;
    
    public void Awake()
    {
        spawnedLensFlare = Instantiate(lensFlarePrefab, transform.position, Quaternion.identity);
        spawnedLensFlare.transform.SetParent(this.transform);
        spawnedLensFlare.transform.localPosition = new Vector3(0f, 0f, -.1f);
        spawnedLensFlare.transform.localScale = new Vector3(canvasSize, canvasSize, canvasSize);
    }
    private void LateUpdate()
    {
        spawnedLensFlare.transform.rotation = Quaternion.identity;
    }
}