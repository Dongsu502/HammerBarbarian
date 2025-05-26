using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class MonsterHitBox : MonoBehaviour
{
    private IMonster monster;

    public BoxCollider hitCollider;

    [SerializeField] private float LightknockbackForce = 1f;
    [SerializeField] private float HeavyknockbackForce = 10f;

    public bool IsKnockback;

    Rigidbody rb;

    private void Awake()
    {
        monster = GetComponentInParent<IMonster>();

        hitCollider = GetComponent<BoxCollider>();

        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Weapon")) 
        {
            //반짝이는 효과
            var flash = GetComponent<HitFlashEffect>();
            if (flash != null)
            {
                flash.Flash();
            }

            AttackType attackType = PlayerHitWhiteBox.WhiteBox.attacktype;
            //강공격인지 확인 -> 넉백
            if(attackType == AttackType.Heavy)
            {
                IsKnockback = true;

                Knockback(other, HeavyknockbackForce);
            }

            if (attackType == AttackType.Light)
            {
                PlayerHitWhiteBox.WhiteBox.Shake(monster.Name, attackType);

                Knockback(other, LightknockbackForce);
            }

            monster.IsHit = true;
        }
    }

    private void Knockback(Collider other, float knockbackForce)
    {
        //나중에 플레이어 무기 타입에 따라서 구분
        Vector3 direction = other.GetComponentInParent<PlayerMove>().gameObject.transform.position - transform.position;

        Vector3 knockbackDir = -direction.normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Debug.Log("피격자에게 넉백 적용: " + knockbackDir);
    }
}
