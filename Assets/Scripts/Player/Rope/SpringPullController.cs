using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopePullController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask pullableLayer;
    [SerializeField] private HammerThrowController hammerThrowController;

    [Header("Pull Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float stopDistance = 1f;

    private Rigidbody rb;
    private Vector3? targetPoint = null;
    private bool isPulling = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (!isPulling || targetPoint == null) return;

        Vector3 toTarget = targetPoint.Value - rb.position;
        float distance = toTarget.magnitude;

        if (distance < stopDistance)
        {
            rb.MovePosition(targetPoint.Value); // 정확히 고정
            StopPull();
            hammerThrowController.Recall();
            return;
        }

        Vector3 direction = toTarget.normalized;
        Vector3 moveStep = direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveStep);
    }

    public void TryStartPull()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, pullableLayer))
        {
            targetPoint = hit.point;
            isPulling = true;
        }
    }

    void StopPull()
    {
        isPulling = false;
        targetPoint = null;
        rb.velocity = Vector3.zero;
    }
}
