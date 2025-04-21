using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 9f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference lookAction;

    private PlayerAttack playerAttack;
    private PlayerIK playerIK;
    private Rigidbody rb;
    private Animator animator;

    private Vector2 inputVector;
    private bool isRunning = false;
    private bool isDiving = false;

    private float currentAnimSpeed = 0f;
    private float diveSpeed = 1f;
    private float diveDuration = 1f;
    private AnimationCurve diveSpeedCurve;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerIK = GetComponent<PlayerIK>();

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

            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, 0f, Time.fixedDeltaTime * 20f);
            animator.SetFloat("MoveSpeed", currentAnimSpeed);

            return;
        }

        Vector3 moveDir = CalculateMoveDirection();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 velocity = moveDir * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        // 💥 핵심 튐 제거 구간
        UpdateMoveSpeed();
    }


    private Vector3 CalculateMoveDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        return (forward.normalized * inputVector.y + right.normalized * inputVector.x).normalized;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
    }

    private void UpdateMoveSpeed()
    {
        if (animator == null) return;

        bool isInputActive = inputVector.sqrMagnitude >= 0.1f;
        bool isIdle = !isInputActive;

        float targetSpeed = isIdle ? 0f : (isRunning ? 1f : 0.5f);

        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, Time.fixedDeltaTime * 20f);
        animator.SetFloat("MoveSpeed", currentAnimSpeed);
    }


    private IEnumerator DiveCoroutine()
    {
        isDiving = true;

        Vector3 moveDir = CalculateMoveDirection();
        if (moveDir == Vector3.zero)
            moveDir = transform.forward;

        transform.rotation = Quaternion.LookRotation(moveDir);
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
        isDiving = false;
    }

    public void EndDive()
    {
        isDiving = false;
    }
}
