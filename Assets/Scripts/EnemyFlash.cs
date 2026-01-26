using System.Collections;
using UnityEngine;

public class EnemyFlash : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private int flashCount = 2;

    private MaterialPropertyBlock propertyBlock;
    private Color baseColor = Color.white;
    private Coroutine flashRoutine;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        propertyBlock = new MaterialPropertyBlock();
        if (targetRenderer != null)
        {
            baseColor = targetRenderer.sharedMaterial != null
                ? targetRenderer.sharedMaterial.color
                : Color.white;
        }
    }

    public void Play()
    {
        if (targetRenderer == null)
        {
            return;
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float singleFlashDuration = Mathf.Max(0.01f, flashDuration / Mathf.Max(1, flashCount));
        for (int i = 0; i < flashCount; i++)
        {
            SetColor(flashColor);
            yield return new WaitForSeconds(singleFlashDuration * 0.5f);
            SetColor(baseColor);
            yield return new WaitForSeconds(singleFlashDuration * 0.5f);
        }

        flashRoutine = null;
    }

    private void SetColor(Color color)
    {
        targetRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", color);
        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}
