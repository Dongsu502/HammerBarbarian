using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackDetection : MonoBehaviour
{
    public Collider AttackCollider;

    private void OnEnable()
    {
        AttackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Debug.Log($"몬스터 {other.gameObject.name}공격!! ");
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = (other.transform.position - transform.position).normalized;

            Player_HitReceiver receiver = other.GetComponent<Player_HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
        }
    }
}
