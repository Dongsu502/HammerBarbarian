using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopePullController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask pullableLayer;

    [Header("Pull Settings")]
    [SerializeField] private float pullForce = 80f;
    [SerializeField] private float maxSpeed = 20f;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPulling)
        {
            TryStartPull();
        }

        if (Input.GetMouseButtonUp(0) && isPulling)
        {
            StopPull();
        }
    }

    void FixedUpdate()
    {
        if (!isPulling || targetPoint == null) return;

        Vector3 toTarget = targetPoint.Value - transform.position;
        float distance = toTarget.magnitude;

        if (distance < stopDistance)
        {
            StopPull();
            return;
        }

        Vector3 direction = toTarget.normalized;
        Vector3 desiredVelocity = direction * maxSpeed;
        Vector3 velocityChange = desiredVelocity - rb.velocity;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void TryStartPull()
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
