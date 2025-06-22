using System.Collections;
using UnityEngine;

public class PlayerBlockHit : MonoBehaviour
{
    private PlayerBlock playerBlock;
    private Rigidbody rb;
    [SerializeField] private GameObject blockEffectPrefab;
    [SerializeField] private Transform blockHitSpawnPos;

    [SerializeField] private float force = 80f;


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
                UIWhiteBox.UseGauge(25f);
                SoundManager.instance.PlayPlayerSFX("Block01");
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Attack_Golem"))
            {
                Debug.Log("막았다! (Golem)");
                PlayBlockEffect();
                ApplySlowKnockbackOverTime();
                SoundManager.instance.PlayPlayerSFX("Block01");
                UIWhiteBox.UseGauge(25f);
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
