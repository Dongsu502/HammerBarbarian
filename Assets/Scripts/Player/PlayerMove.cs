using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 9f;
    [SerializeField] private Transform cameraTransform;
    private float currentAnimSpeed = 0f;

    [Header("Animation")]
    private Animator animator;

    private Rigidbody rb;
    private Vector2 inputVector;
    private bool isRunning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator가 할당되지 않았습니다!");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isRunning = true;
        }
        
        if(context.canceled) 
        {
            isRunning= false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = CalculateMoveDirection();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 velocity = moveDirection * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

            // 애니메이션 파라미터 전달 (Blend Tree 연결 시 사용)
        if (animator != null)
        {
            float flatSpeed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, flatSpeed, Time.fixedDeltaTime * 10f);
            animator.SetFloat("MoveSpeed", currentAnimSpeed);
        }
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        return (camForward * inputVector.y + camRight * inputVector.x).normalized;
    }
}
