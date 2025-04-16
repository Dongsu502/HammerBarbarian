using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -5);
    [SerializeField] private InputActionReference lookAction;

    private float yaw;
    private Vector2 lookInput;

    [ContextMenu("test")]
    public void test()
    {
        DisableActionLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * mouseSensitivity;

        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        transform.position = player.position + rotation * offset;
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }

    public void EnableActionLook() => lookAction.action.Enable();
    public void DisableActionLook() => lookAction.action.Disable();
}
