using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour
{
    private Monster monster;

    public BoxCollider hitCollider;

    public float knockbackForce = 10f;
    public float knockbackDuration = 2.1f;
    public bool IsKnockback;

    public int powerAttack;

    Rigidbody rb;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();

        hitCollider = GetComponent<BoxCollider>();

        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Weapon")) 
        {
            //강공격인지 확인 -> 넉백
            if(powerAttack == 3)
            {
                //넉백중이라면 취소
                if (IsKnockback) return;

                IsKnockback = true;
                monster.KnocbackActive(false);

                Knockback(other);
            }

            hitCollider.enabled = false;
            monster.TakeDamage(10);
        }
    }

    private void Knockback(Collider other)
    {
        Vector3 direction = other.transform.position - transform.position;

        Vector3 knockbackDir = -direction.normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Debug.Log("피격자에게 넉백 적용: " + knockbackDir);
    }
}
