using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitReceiver : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMove playerMove;

    [Header("Hit Reaction")]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float knockbackDuration = 0.3f;

    private bool isKnockedBack = false;


    /// <summary>
    /// 피격 시 호출되는 메서드 (공격자 쪽에서 방향 정보 전달 필요)
    /// </summary>
    public void OnHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isKnockedBack) return;

        Vector3 knockDir = hitNormal.normalized;
        StartCoroutine(KnockbackCoroutine(knockDir));
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction)
    {
        isKnockedBack = true;

        // 이동/입력 차단
        if (playerMove != null)
            playerMove.enabled = false;

        // 초기화
        direction.y = 0f;
        direction.Normalize();
        animator.applyRootMotion = false;

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            Vector3 offset = direction * knockbackForce * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + offset);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.velocity = Vector3.zero;
        animator.applyRootMotion = true;

        if (playerMove != null)
            playerMove.enabled = true;

        isKnockedBack = false;
    }

}
