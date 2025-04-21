using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class MonsterHitBox : MonoBehaviour
{
    public string MonsterName = "Golem";
    private Monster monster;

    public BoxCollider hitCollider;

    public float knockbackForce = 10f;
    public float knockbackDuration = 2.1f;
    public bool IsKnockback;

    private const int POWER_ATTACKTYPE = 2;

    public bool testAttackType;

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
            AttackType attackType = PlayerHitWhiteBox.WhiteBox.attacktype;
            //강공격인지 확인 -> 넉백
            if(PlayerWhiteBox.WhiteBox.currentAttackType == POWER_ATTACKTYPE || testAttackType)
            {
                //넉백중이라면 취소
                if (IsKnockback) return;

                IsKnockback = true;
                monster.KnocbackActive(false);

                Knockback(other);
            }

            if (attackType == AttackType.Light)
            {
                PlayerHitWhiteBox.WhiteBox.Shake(MonsterName, attackType);
            }



            hitCollider.enabled = false;
            monster.TakeDamage(10);
        }
    }

    private void Knockback(Collider other)
    {
        Vector3 direction = other.GetComponentInParent<PlayerMove>().gameObject.transform.position - transform.position;

        Vector3 knockbackDir = -direction.normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Debug.Log("피격자에게 넉백 적용: " + knockbackDir);
    }
}
