using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [Header("파괴 설정")]
    [Tooltip("부서진 프리팹")]
    public GameObject brokenPrefab;

    [Tooltip("파편에 가할 힘")]
    public float explosionForce = 10f;

    [Tooltip("파편이 사라지는 시간")]
    public float debrisLifetime = 5f;

    [Tooltip("파편이 약간 위로 생성되는 높이")]
    public float spawnOffset = 0.1f;

    private Transform player;
    public bool isBroken = false;

    [SerializeField] private ObjectBrokenTrigger trigger;

    private Renderer renderer;

    private void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("BreakableObject: 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다.");

        renderer= GetComponent<Renderer>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isBroken || player == null) return;

        Debug.Log("무기와 충돌 감지됨: " + other.name);

        if (other.CompareTag("Weapon"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 forceDir = player.forward.normalized;

            Break(hitPoint, forceDir, other);
        }
    }

    private void Break(Vector3 hitPoint, Vector3 forceDirection, Collider weaponCollider)
    {
        isBroken = true;

        // 파편 먼저 생성하고
        Vector3 spawnPos = transform.position + Vector3.up * spawnOffset;
        Debug.Log("파편 생성 시도됨");
        GameObject brokenObject = Instantiate(brokenPrefab, spawnPos, transform.rotation);
        Debug.Log("파편 생성 완료");

        // BrokenObjectController 가져와서 힘 주기
        var controller = brokenObject.GetComponent<BrokenObjectController>();
        if (controller != null)
        {
            controller.baseForceDirection = forceDirection;  // 힘 방향 전달
            controller.Explode();
        }


        // 무기 충돌 무시 처리 → DebrisController 통해
        DebrisController debrisController = brokenObject.GetComponent<DebrisController>();
        if (debrisController != null)
        {
            debrisController.IgnoreWeaponCollision(weaponCollider);
        }

        // 원본 비활성화는 마지막에
        renderer.enabled = false;
        StartCoroutine(DelayTrigger());
        Destroy(brokenObject, debrisLifetime);
    }

    private IEnumerator DelayTrigger()
    {
        yield return new WaitForSeconds(2.5f);
        trigger.BrokenTrigger();
        gameObject.SetActive(false);
    }

    private IEnumerator IgnoreWeaponCollisionNextFrame(GameObject brokenObject, Collider weaponCollider)
    {
        yield return null; // 1프레임 대기

        Collider[] debrisColliders = brokenObject.GetComponentsInChildren<Collider>();
        foreach (var col in debrisColliders)
        {
            Physics.IgnoreCollision(col, weaponCollider);
        }
    }
}