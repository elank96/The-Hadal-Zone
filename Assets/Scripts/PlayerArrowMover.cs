using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerArrowMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody rb3D;

    private void Awake()
    {
        if (rb3D == null)
        {
            rb3D = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            float x = (Keyboard.current.rightArrowKey.isPressed ? -1f : 0f)
                      + (Keyboard.current.leftArrowKey.isPressed ? 1f : 0f);
            float y = (Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
                      + (Keyboard.current.downArrowKey.isPressed ? -1f : 0f);
            input = new Vector2(x, y);
            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }
        }

        Vector3 moveDelta = new Vector3(input.x, input.y, 0f) * (moveSpeed * Time.deltaTime);

        if (rb3D != null)
        {
            Vector3 currentPos = rb3D.position;
            rb3D.MovePosition(new Vector3(currentPos.x + moveDelta.x, currentPos.y + moveDelta.y, currentPos.z));
        }
        else
        {
            Vector3 currentPos = transform.position;
            transform.position = new Vector3(currentPos.x + moveDelta.x, currentPos.y + moveDelta.y, currentPos.z);
        }
    }
}
