using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private AimCameraSwitcher cameraSwitcher;
    private PlayerStatus status;

    private PlayerAttack playerAttack;
    private PlayerIK playerIK;
    private Rigidbody rb;
    private Animator animator;

    private Vector2 inputVector;
    private bool isDiving = false;

    public float currentAnimSpeed = 0f;
    [SerializeField]private float diveSpeed = 5f;
    private float diveDuration = 0.38f;
    private AnimationCurve diveSpeedCurve;

    private void Awake()
    {
        status = GetComponent<PlayerStatus>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerIK = GetComponent<PlayerIK>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (status.IsDead) return;
        Debug.LogWarning("움직일 수 있어!");

        inputVector = context.ReadValue<Vector2>();
    }

    public void OnDIve(InputAction.CallbackContext context)
    {
        if (status.IsDead) return;

        if (context.performed && !isDiving && !playerAttack.IsAttackAnim())
        {
            animator.applyRootMotion = true;

            animator.SetTrigger("Dive");
        }
    }

    private void FixedUpdate()
    {
        if (status.IsDead) return;

        if (isDiving || playerAttack.IsAttackAnim())
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);

            currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, 0f, Time.fixedDeltaTime * 20f);
            animator.SetFloat("MoveSpeed", currentAnimSpeed);

            return;
        }

        Vector3 moveDir = CalculateMoveDirection();
        float currentSpeed = cameraSwitcher.isAiming ? 5f : moveSpeed;

        Vector3 velocity = moveDir * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (moveDir.sqrMagnitude > 0.01f && !cameraSwitcher.isAiming)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        UpdateMoveSpeed();
        if (!isDiving)
        {
            StickToGround();

        }
    }

    private void StickToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            Vector3 position = transform.position;
            position.y = Mathf.MoveTowards(position.y, hit.point.y, 5f * Time.fixedDeltaTime);
            transform.position = position;
        }
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        return (forward.normalized * inputVector.y + right.normalized * inputVector.x).normalized;
    }

    private void UpdateMoveSpeed()
    {
        if (animator == null) return;

        bool isInputActive = inputVector.sqrMagnitude >= 0.1f;
        float targetSpeed = isInputActive ? 0.8f : 0f;

        currentAnimSpeed = Mathf.Lerp(currentAnimSpeed, targetSpeed, Time.fixedDeltaTime * 5f);
        animator.SetFloat("MoveSpeed", currentAnimSpeed);
    }

    private IEnumerator DiveCoroutine()
    {
        isDiving = true;

        Vector3 moveDir = CalculateMoveDirection();
        if (moveDir == Vector3.zero)
            moveDir = transform.forward;

        transform.rotation = Quaternion.LookRotation(moveDir);
        UIWhiteBox.UseGauge(30f);

        float elapsed = 0f;
        while (elapsed < diveDuration)
        {
            Vector3 offset = moveDir * diveSpeed * Time.fixedDeltaTime;

            // 벽 충돌 사전 감지
            if (Physics.Raycast(rb.position, moveDir, out RaycastHit hit, offset.magnitude + 0.1f, LayerMask.GetMask("InvisibleWall")))
            {
                break;
            }

            rb.MovePosition(rb.position + offset);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        animator.applyRootMotion = false;
    }

    public void StartDive()
    {
        isDiving = true;
    }

    public void EndDive()
    {
        isDiving = false;
    }

    public void OnInterpolate()
    {
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        //rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public void OffInterpolate()
    {
        rb.interpolation = RigidbodyInterpolation.None;
        //rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    }
}
