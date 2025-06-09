using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HitReceiver : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject player;
    private Collider playerHitBox;
    private PlayerAttack playerAttack;

    [Header("Hit Reaction")]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float knockbackDuration = 0.3f;

    private bool isKnockedBack = false;

    private void Awake()
    {
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerHitBox = GetComponent<CapsuleCollider>();
    }

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
        playerAttack.DisableHammerCollider();
        isKnockedBack = true;

        if (playerMove != null)
            playerMove.enabled = false;

        animator.SetTrigger("Hit");

        direction.y = 0f;
        direction.Normalize();
        animator.applyRootMotion = false;

        // 회전 처리 (넉백 반대방향 = 맞은 방향)
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(-direction);
            lookRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);
            player.transform.rotation = lookRot;
        }

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            Vector3 offset = direction * knockbackForce * Time.fixedDeltaTime;
            Vector3 backDir = -direction;

            if (Physics.Raycast(rb.position, backDir, out RaycastHit hit, offset.magnitude + 0.1f, LayerMask.GetMask("InvisibleWall")))
            {
                break;
            }

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

    public void OnHitBox()
    {
        playerHitBox.enabled = true;
    }

    public void OffHitBox()
    {
        playerHitBox.enabled = false;
    }

}
