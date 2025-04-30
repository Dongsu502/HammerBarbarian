using UnityEngine;
using System.Collections;

public class HammerThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform throwOrigin;
    public GameObject hammerPrefab;
    public Camera mainCamera;

    [Header("Throw Settings")]
    public float throwDistance = 15f;
    public float throwSpeed = 30f;

    [Header("Recall Settings")]
    public float recallSpeed = 40f;
    public AnimationCurve recallEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Camera Aim Offset")]
    public float verticalOffset = 0.65f;

    private GameObject activeHammer;
    private Rigidbody hammerRb;

    private bool isThrowing = false;
    private bool isRecalling = false;

    private Vector3 throwStart;
    private Vector3 throwTarget;
    private float throwTimer;

    public GameObject ActiveHammer => activeHammer;

    public Transform ActiveRopeAttachPoint
    {
        get
        {
            if (activeHammer == null) return null;
            return activeHammer.transform.Find("RopeAttachPoint");
        }
    }

    [ContextMenu("던지기")]
    public void Throw()
    {
        if (isThrowing || isRecalling || activeHammer != null) return;

        activeHammer = Instantiate(hammerPrefab, throwOrigin.position, hammerPrefab.transform.rotation);

        hammerRb = activeHammer.GetComponent<Rigidbody>();
        hammerRb.isKinematic = true;

        // 회수 전까지 충돌 무시 (충돌 막기용)
        activeHammer.layer = LayerMask.NameToLayer("Ignore Raycast");

        throwStart = activeHammer.transform.position;

        var ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, verticalOffset, 0));
        Vector3 direction = ray.direction.normalized;
        throwTarget = throwStart + direction * throwDistance;

        throwTimer = 0f;
        isThrowing = true;
    }

    [ContextMenu("회수")]
    public void Recall()
    {
        if (isRecalling || activeHammer == null) return;

        isThrowing = false;
        isRecalling = true;
        StartCoroutine(RecallRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) Throw();
        if (Input.GetKeyDown(KeyCode.R)) Recall();
    }

    void FixedUpdate()
    {
        if (isThrowing && hammerRb != null)
        {
            throwTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01((throwTimer * throwSpeed) / throwDistance);
            Vector3 newPos = Vector3.Lerp(throwStart, throwTarget, t);
            hammerRb.MovePosition(newPos);

            if (t >= 1f)
            {
                isThrowing = false;
                //물리 충돌은 여기서 비활성화된 상태 유지!
            }
        }
    }

    private IEnumerator RecallRoutine()
    {
        hammerRb.isKinematic = true;

        Vector3 start = activeHammer.transform.position;
        Vector3 end = throwOrigin.position;
        float duration = Vector3.Distance(start, end) / recallSpeed;

        float timer = 0f;
        while (timer < duration)
        {
            float t = recallEase.Evaluate(timer / duration);
            Vector3 pos = Vector3.Lerp(start, end, t);
            hammerRb.MovePosition(pos);

            timer += Time.deltaTime;
            yield return null;
        }

        hammerRb.MovePosition(end);

        // 회수 완료 시에만 충돌 활성화
        hammerRb.isKinematic = false;
        activeHammer.layer = LayerMask.NameToLayer("Hammer");

        Destroy(activeHammer);
        activeHammer = null;
        hammerRb = null;
        isRecalling = false;
    }
}
