using UnityEngine;

public class HammerThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform throwOrigin;
    public GameObject hammerPrefab;
    public Camera mainCamera;
    [SerializeField] private HammerThrowAnimHandler hammerThrowAnimHandler;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRb; // 플레이어 끌기용

    [Header("Throw Settings")]
    public float throwSpeed = 30f;
    public float maxThrowDistance = 15f;
    public float verticalOffset = 0.65f;

    [Header("Recall Settings")]
    public float recallForce = 80f;
    public float maxRecallSpeed = 25f;
    public float stopDistance = 0.5f;

    [Header("Rope Pull Settings")]
    [SerializeField] private float ropePullStrength = 1f;
    [SerializeField] private float ropeDamper = 5f;
    [SerializeField] private float ropeMaxDistance = 2f;
    [SerializeField] private float ropeStopDistance = 1f;

    private GameObject activeHammer;
    private Rigidbody hammerRb;
    private SpringJoint ropeJoint;

    private bool isThrowing = false;
    private bool isRecalling = false;

    private Vector3 throwStartPos;

    private Vector3 pullPoint;

    public GameObject ActiveHammer => activeHammer;

    public void Throw()
    {
        if (activeHammer != null || isThrowing || isRecalling) return;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, verticalOffset, 0));
        Vector3 dir = ray.direction.normalized;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            pullPoint = hit.point;
        }

            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        Vector3 euler = lookRotation.eulerAngles;
        euler.x = -90f;
        Quaternion finalRotation = Quaternion.Euler(euler);

        activeHammer = Instantiate(hammerPrefab, throwOrigin.position, finalRotation);
        hammerRb = activeHammer.GetComponent<Rigidbody>();

        var collisionHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();
        if (collisionHandler != null)
        {
            collisionHandler.SetController(this);
        }

        hammerRb.isKinematic = false;
        hammerRb.velocity = Vector3.zero;
        hammerRb.angularVelocity = Vector3.zero;

        hammerRb.AddForce(dir * throwSpeed, ForceMode.VelocityChange);
        throwStartPos = activeHammer.transform.position;

        hammerThrowAnimHandler.StartThrow();
        isThrowing = true;
    }

    public void Recall()
    {
        if (activeHammer == null || isRecalling) return;

        isThrowing = false;
        isRecalling = true;

        if (hammerRb != null)
        {
            hammerRb.isKinematic = true;
        }
    }

    public void StartRopePull(Vector3 targetPoint)
    {
        if (ropeJoint != null)
        {
            Destroy(ropeJoint);
        }

        ropeJoint = playerRb.gameObject.AddComponent<SpringJoint>();
        ropeJoint.autoConfigureConnectedAnchor = false;
        ropeJoint.connectedAnchor = targetPoint;

        ropeJoint.spring = ropePullStrength;
        ropeJoint.damper = ropeDamper;
        ropeJoint.maxDistance = ropeMaxDistance;
        ropeJoint.minDistance = 0f;
        ropeJoint.enableCollision = false;

        Debug.Log("[RopePull] 플레이어 끌기 시작");
    }

    private void FixedUpdate()
    {
        if (isThrowing && hammerRb != null && activeHammer != null)
        {
            float distance = Vector3.Distance(throwStartPos, activeHammer.transform.position);

            //벽에 박힌 경우 자동 회수 금지
            var collisionHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();
            if (collisionHandler != null && collisionHandler.IsStuckToWall)
                return;

            if (distance >= maxThrowDistance)
            {
                isThrowing = false;

                hammerRb.velocity = Vector3.zero;
                hammerRb.angularVelocity = Vector3.zero;
                hammerRb.isKinematic = false;

                Recall(); // 자동 회수
            }
        }

        if (isRecalling && hammerRb != null)
        {
            Vector3 toHand = throwOrigin.position - hammerRb.position;
            float distance = toHand.magnitude;

            if (distance <= stopDistance)
            {
                if (hammerRb != null)
                {
                    hammerRb.isKinematic = true;
                }

                Destroy(activeHammer);
                activeHammer = null;
                hammerRb = null;
                isRecalling = false;
                hammerThrowAnimHandler.StopThrow();
                return;
            }

            Vector3 direction = toHand.normalized;
            hammerRb.position += direction * maxRecallSpeed * Time.fixedDeltaTime;

            if (hammerRb.velocity.magnitude > maxRecallSpeed)
            {
                hammerRb.velocity = hammerRb.velocity.normalized * maxRecallSpeed;
            }
        }

        // SpringJoint 로프 이동 중 도착 처리
        if (ropeJoint != null)
        {
            float dist = Vector3.Distance(playerRb.position, ropeJoint.connectedAnchor);
            if (dist < ropeStopDistance)
            {
                Destroy(ropeJoint);
                ropeJoint = null;
                Debug.Log("[RopePull] 도착 및 해제");
            }
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.M)) 
        {
            StartRopePull(pullPoint);
        }
        if (activeHammer != null && Input.GetMouseButtonDown(0))
        {
            var stuckHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();
            if (stuckHandler != null && stuckHandler.IsStuckToWall)
            {
                Debug.Log("회수!!");
                stuckHandler.UnstickFromWall();
                Recall();
            }
        }
    }
}
