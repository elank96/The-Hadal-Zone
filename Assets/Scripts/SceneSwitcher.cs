using UnityEngine;
using UnityEngine.SceneManagement; // Essential for scene control

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] public string sceneToLoad;
    [SerializeField] private LayerMask playerLayer;

    // Optional: Add a delay like a debounce timer in hardware
    [SerializeField] private float transitionDelay = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        // Bitwise check for the player layer
        if ((playerLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            // We use Invoke to create a small buffer for cleanup
            Invoke(nameof(LoadTargetScene), transitionDelay);
        }
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name not specified on " + gameObject.name);
        }
    }
}