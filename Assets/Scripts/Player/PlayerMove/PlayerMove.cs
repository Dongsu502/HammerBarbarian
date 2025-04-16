using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 9f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    private PlayerAttack playerAttack;
    [SerializeField] private InputActionReference lookAction;

    private Rigidbody rb;
    private Animator animator;

    private Vector2 inputVector;
    private bool isRunning = false;
    private float currentAnimSpeed = 0f;

    private float diveSpeed = 1f;
    private float diveDuration =1f;
    private AnimationCurve diveSpeedCurve;
    private bool isDiving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();

        // MainCamera의 Transform을 자동 참조
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.performed;
    }

    public void OnDIve(InputAction.CallbackContext context)
    {
        if (context.performed && !isDiving && !playerAttack.IsAttackAnim())
        {
            animator.applyRootMotion = true;
            StartCoroutine(DiveCoroutine());
        }
    }

    private void FixedUpdate()
    {
        if (isDiving || playerAttack.IsAttackAnim())
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            animator.SetFloat("MoveSpeed", 0f);
            return;
        }

        Vector3 moveDirection = CalculateMoveDirection();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 velocity = moveDirection * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        if (animator != null)
        {
            float flatSpeed = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, flatSpeed, Time.fixedDeltaTime * 10f);
            animator.SetFloat("MoveSpeed", currentAnimSpeed);
        }
    }

    private Vector3 CalculateMoveDirection()
    {
        // 카메라 방향 기반 이동 방향 계산
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        return (camForward * inputVector.y + camRight * inputVector.x).normalized;
    }

    private IEnumerator DiveCoroutine()
    {
        isDiving = true;

        Vector3 moveDir = CalculateMoveDirection();
        if(moveDir == Vector3.zero)
        {
            moveDir =transform.forward;
        }

        Quaternion diveRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = diveRotation;

        animator.SetTrigger("Dive");
        UIWhiteBox.UseGauge(30f);

        float elapsed = 0f;
        while (elapsed < diveDuration)
        {
            Vector3 offset = moveDir * diveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + offset);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        animator.applyRootMotion = false;
    }

    public void EndDive()
    {
        isDiving = false;
    }
}
