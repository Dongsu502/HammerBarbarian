using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackDetection : MonoBehaviour
{
    public string monsterName;
    public int monsterAttackPower;
    public GameObject AttackCollider;

    private void Start()
    {
        if(monsterName == "Golem")
        {
            AttackCollider.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Debug.Log($"몬스터 {other.gameObject.name}공격!! ");
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = (other.transform.position - transform.position).normalized;

            if (monsterName == "Mushroom")
            {
                gameObject.GetComponent<BulletEffect>().SpawnHitEffect(hitPoint);
                Destroy(gameObject);
            }

            Player_HitReceiver receiver = other.GetComponent<Player_HitReceiver>();
            if (receiver != null)
            {
                receiver.OnHit(hitPoint, hitNormal);
            }
        }
        
    }
}
