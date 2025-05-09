using UnityEngine;

public class HammerThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform throwOrigin;
    public GameObject hammerPrefab;
    public Camera mainCamera;

    [Header("Throw Settings")]
    public float throwSpeed = 30f;
    public float maxThrowDistance = 15f;
    public float verticalOffset = 0.65f;

    [Header("Recall Settings")]
    public float recallForce = 80f;
    public float maxRecallSpeed = 25f;
    public float stopDistance = 0.5f;

    private GameObject activeHammer;
    private Rigidbody hammerRb;

    private bool isThrowing = false;
    private bool isRecalling = false;

    private Vector3 throwStartPos;

    public GameObject ActiveHammer => activeHammer;

    public void Throw()
    {
        if (activeHammer != null || isThrowing || isRecalling) return;

        activeHammer = Instantiate(hammerPrefab, throwOrigin.position, hammerPrefab.transform.rotation);
        hammerRb = activeHammer.GetComponent<Rigidbody>();

        var collisionHandler = activeHammer.GetComponent<RopeWeaponCollisionHandler>();
        if (collisionHandler != null)
        {
            collisionHandler.SetController(this);
        }

        hammerRb.isKinematic = false;
        hammerRb.velocity = Vector3.zero;
        hammerRb.angularVelocity = Vector3.zero;

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, verticalOffset, 0));
        Vector3 dir = ray.direction.normalized;

        hammerRb.AddForce(dir * throwSpeed, ForceMode.VelocityChange);
        throwStartPos = activeHammer.transform.position;

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

    private void FixedUpdate()
    {
        if (isThrowing && hammerRb != null && activeHammer != null)
        {
            float distance = Vector3.Distance(throwStartPos, activeHammer.transform.position);

            if (distance >= maxThrowDistance)
            {
                isThrowing = false;

                hammerRb.velocity = Vector3.zero;
                hammerRb.angularVelocity = Vector3.zero;
                hammerRb.isKinematic = false;

                // 자동 회수
                Recall();
            }
        }

        if (isRecalling && hammerRb != null)
        {
            Vector3 toHand = throwOrigin.position - hammerRb.position;
            float distance = toHand.magnitude;

            if (distance <= stopDistance)
            {
                hammerRb.velocity = Vector3.zero;
                hammerRb.angularVelocity = Vector3.zero;
                Destroy(activeHammer);
                activeHammer = null;
                hammerRb = null;
                isRecalling = false;
                return;
            }

            Vector3 direction = toHand.normalized;
            hammerRb.position += direction * maxRecallSpeed * Time.fixedDeltaTime;

            //Vector3 direction = toHand.normalized;
            //hammerRb.AddForce(direction * recallForce, ForceMode.Force);

            // 속도 제한
            if (hammerRb.velocity.magnitude > maxRecallSpeed)
            {
                hammerRb.velocity = hammerRb.velocity.normalized * maxRecallSpeed;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) Recall();
    }
}
