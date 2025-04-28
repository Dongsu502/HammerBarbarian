using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            Debug.Log($"몬스터 {other.gameObject.name}공격!! ");
        }
    }
}
