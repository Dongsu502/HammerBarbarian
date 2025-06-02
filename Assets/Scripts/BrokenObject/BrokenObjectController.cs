using UnityEngine;

public class BrokenObjectController : MonoBehaviour
{
    public float explosionForce = 10f;
    public float spreadAngle = 45f;

    // 플레이어가 보는 방향 반대 방향으로 기본 힘 방향 설정 가능
    public Vector3 baseForceDirection = Vector3.back;

    public void Explode()
    {
        Rigidbody[] childRigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in childRigidbodies)
        {
            // 최상위 위치 기준으로 힘 방향에 랜덤 퍼짐 적용
            Vector3 randomDir = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle)
            ) * baseForceDirection;

            float randomForce = Random.Range(explosionForce * 0.7f, explosionForce * 1.3f);

            rb.AddForce(randomDir.normalized * randomForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * randomForce, ForceMode.Impulse);
        }
    }
}
