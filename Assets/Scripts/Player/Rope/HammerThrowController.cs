using UnityEngine;
using System.Collections;

public class HammerThrowController : MonoBehaviour
{
    [Header("References")]
    public Transform throwOrigin;               // 손 위치
    public GameObject hammerPrefab;             // 해머 프리팹
    public Camera mainCamera;                   // 카메라 (인스펙터에서 직접 연결)

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

    private Vector3 throwStart;
    private Vector3 throwTarget;
    private float throwTimer;

    private bool isThrowing = false;
    private bool isRecalling = false;

    public GameObject ActiveHammer => activeHammer;

    [ContextMenu("던지기")]
    public void Throw()
    {
        if (activeHammer != null || isThrowing || isRecalling) return;

        // 해머 생성
        activeHammer = Instantiate(hammerPrefab, throwOrigin.position, hammerPrefab.transform.rotation);
        hammerRb = activeHammer.GetComponent<Rigidbody>();
        hammerRb.isKinematic = true; // MovePosition 사용할 것이므로

        throwStart = activeHammer.transform.position;

        // 카메라 중심 기준 방향
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, verticalOffset, 0));
        Vector3 dir = ray.direction.normalized;
        throwTarget = throwStart + dir * throwDistance;

        throwTimer = 0f;
        isThrowing = true;
    }

    [ContextMenu("회수")]
    public void Recall()
    {
        if (activeHammer == null || isRecalling) return;

        isThrowing = false;
        isRecalling = true;
        StartCoroutine(RecallRoutine());
    }

    private void FixedUpdate()
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

                hammerRb.isKinematic = false;
            }
        }
    }

    private IEnumerator RecallRoutine()
    {
        hammerRb.isKinematic = true; // 충돌 무시하고 회수

        Vector3 start = activeHammer.transform.position;
        Vector3 end = throwOrigin.position;
        float duration = Vector3.Distance(start, end) / recallSpeed;

        float timer = 0f;
        while (timer < duration)
        {
            float t = recallEase.Evaluate(timer / duration);
            Vector3 pos = Vector3.Lerp(start, end, t);
            activeHammer.transform.position = pos;

            timer += Time.deltaTime;
            yield return null;
        }

        activeHammer.transform.position = end;

        Destroy(activeHammer);
        activeHammer = null;
        hammerRb = null;
        isRecalling = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) Throw();
        if (Input.GetKeyDown(KeyCode.R)) Recall();
    }
}
