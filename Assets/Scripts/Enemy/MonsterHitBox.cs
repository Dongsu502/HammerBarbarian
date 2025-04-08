using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitBox : MonoBehaviour
{
    private Monster monster;

    public BoxCollider hitCollider;

    public float knockbackForce = 10f;
    public float knockbackDuration = 1f;
    public bool IsKnockback;

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
            if(IsKnockback)
            {
                return;
            }

            IsKnockback = true;
            monster.SetAgentEnable(false);
            rb.isKinematic = false;

            Knockback(other);

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
