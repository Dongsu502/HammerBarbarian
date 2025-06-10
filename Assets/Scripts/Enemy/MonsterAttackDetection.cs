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
        //플레이어 히트
        if(other.transform.CompareTag("PlayerHitBox"))
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

        //플레이어 막기 시전
        if(other.transform.CompareTag("Player_BlockHitBox"))
        {
            Debug.Log($"{other.gameObject.name}에 공격이 막혔다!");
            
            if (monsterName == "Golem")
            {
                AttackCollider.SetActive(false);
            }
            else if (monsterName == "Mushroom")
            {
                Destroy(gameObject);
            }
        }
    }
}
