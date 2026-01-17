using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scene References")]
    public Transform player;
    [SerializeField] private Camera targetCamera;

    [Header("Scroll Settings")]
[SerializeField] private float scrollSpeed = 5f;
[SerializeField] private bool followPlayerVertically = false;
    [SerializeField] private float resetThresholdY = 0f;
    [SerializeField] private float resetToY = 19f;

    private Rigidbody2D playerRb2D;
    private Vector3 cameraOffset;

    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (player != null)
        {
            playerRb2D = player.GetComponent<Rigidbody2D>();
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (player != null && targetCamera != null)
        {
            cameraOffset = targetCamera.transform.position - player.position;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            return;
        }

        Vector3 delta = Vector3.down * scrollSpeed * Time.deltaTime;

        if (playerRb2D == null)
        {
            player.position += delta;
            ResetPlayerIfBelowThreshold();
        }
    }

    private void FixedUpdate()
    {
        if (playerRb2D == null)
        {
            return;
        }

        Vector2 delta = Vector2.down * scrollSpeed * Time.fixedDeltaTime;
        playerRb2D.MovePosition(playerRb2D.position + delta);
        ResetPlayerIfBelowThreshold();
    }

    private void LateUpdate()
    {
        if (player == null || targetCamera == null)
        {
            return;
        }

    Vector3 camPos = player.position + cameraOffset;
    if (!followPlayerVertically)
    {
        camPos.y = targetCamera.transform.position.y;
    }

    targetCamera.transform.position = camPos;
    }

    private void ResetPlayerIfBelowThreshold()
    {
        if (player.position.y > resetThresholdY)
        {
            return;
        }

        Vector3 pos = player.position;
        pos.y = resetToY;

        if (playerRb2D != null)
        {
            playerRb2D.position = pos;
        }
        else
        {
            player.position = pos;
        }
    }

}
