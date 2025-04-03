using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 3f;

    // 카메라가 위에서 내려다보도록 적절한 offset
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -5);

    private float yaw;
    private Vector2 lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void LateUpdate()
    {
        yaw += lookInput.x * mouseSensitivity;

        // Yaw만 반영해서 회전
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);

        // 위에서 뒤로 빠진 위치에 카메라 배치
        transform.position = player.position + rotation * offset;

        // 플레이어의 중간~머리 방향을 보게 설정
        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
