using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackDetection : MonoBehaviour
{
    [Tooltip("몬스터 공격 콜라이더")]
    public BoxCollider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Debug.Log($"몬스터 {other.gameObject.name}공격!! ");
        }
    }
}
