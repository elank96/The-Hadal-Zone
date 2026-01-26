using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float maxAlpha = 0.8f;

    private CanvasGroup canvasGroup;
    private Coroutine flashRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Play()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }

        flashRoutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        float halfDuration = Mathf.Max(0.01f, flashDuration * 0.5f);
        float t = 0f;

        while (t < halfDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, maxAlpha, t / halfDuration);
            yield return null;
        }

        t = 0f;
        while (t < halfDuration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(maxAlpha, 0f, t / halfDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        flashRoutine = null;
    }
}
