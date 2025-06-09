using System.Collections;
using UnityEngine;

public class PlayerBlockHit : MonoBehaviour
{
    private PlayerBlock playerBlock;
    private Rigidbody rb;
    [SerializeField] private GameObject blockEffectPrefab;
    [SerializeField] private Transform blockHitSpawnPos;


    private void Awake()
    {
        playerBlock = GetComponentInParent<PlayerBlock>();
        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerBlock != null)
        {
            if (other.CompareTag("Attack_Mushroom"))
            {
                Debug.Log("막았다! (Mushroom)");
                PlayBlockEffect();
                ApplySlowKnockbackOverTime();
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Attack_Golem"))
            {
                Debug.Log("막았다! (Golem)");
                PlayBlockEffect();
                ApplySlowKnockbackOverTime();
            }
        }
    }

    private void ApplySlowKnockbackOverTime()
    {
        StartCoroutine(SlowKnockbackCoroutine());
    }

    private IEnumerator SlowKnockbackCoroutine()
    {
        float duration = 0.15f; // 밀리는 시간
        float timer = 0f;

        Vector3 direction = -transform.forward;
        direction.y = 0f; // 살짝 위로

        float force = 80f; // mass 15 기준으로 적당히 천천히 밀릴 정도

        while (timer < duration)
        {
            rb.AddForce(direction.normalized * force * Time.fixedDeltaTime, ForceMode.VelocityChange);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    private void PlayBlockEffect()
    {
        if (blockEffectPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 1f; // 플레이어 위쪽 약간
        Quaternion spawnRot = Quaternion.LookRotation(-transform.forward); // 뒤쪽으로 회전

        Instantiate(blockEffectPrefab, blockHitSpawnPos.position, spawnRot);
    }
}
