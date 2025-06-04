using System.Collections;
using UnityEngine;

public class HitStopHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float hitStopCooldown = 0.1f; // 쿨타임 추가

    private bool isHitStopping = false;
    private float lastHitStopTime = -999f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HitStop(float duration)
    {
        // 쿨타임 체크
        if (isHitStopping || Time.time - lastHitStopTime < hitStopCooldown)
            return;

        StartCoroutine(HitStopCoroutine(duration));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        isHitStopping = true;
        lastHitStopTime = Time.time;

        float originalSpeed = animator.speed;
        animator.speed = 0f;

        yield return new WaitForSeconds(duration);

        animator.speed = originalSpeed;
        isHitStopping = false;
    }
}
