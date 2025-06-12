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

    private Vector3 hitDirection;

    Rigidbody rb;

    public AttackType playerAttackType;

    public bool isTriggerHit;

    private void Awake()
    {
        monster = GetComponentInParent<IMonster>();

        hitCollider = GetComponent<BoxCollider>();

        rb = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Weapon") && !isTriggerHit) 
        {
            isTriggerHit = true;

            //반짝이는 효과
            var flash = GetComponent<HitFlashEffect>();
            if (flash != null)
            {
                flash.Flash();
            }

            rb.isKinematic = false;

            //플레이어 공격타입 확인
            playerAttackType = PlayerHitWhiteBox.WhiteBox.attacktype;
            AttackTypeCheck(other, playerAttackType);

            monster.IsHit = true;
        }
    }

    private void AttackTypeCheck(Collider other, AttackType newAttackType)
    {
        float knockbackForce = 0f;

        if(newAttackType == AttackType.None)
        {
            Debug.LogError("AttackType이 None입니다.");
            return;
        }
        //약공격
        if(newAttackType == AttackType.Light || newAttackType == AttackType.WhirlWind)
        {
            knockbackForce = LightknockbackForce;
        }
        //강공격
        if(newAttackType == AttackType.Heavy || newAttackType == AttackType.Skill)
        {
            IsKnockback = true;
            knockbackForce = HeavyknockbackForce;
        }

        //카메라 흔들림 효과
        PlayerHitWhiteBox.WhiteBox.Shake(monster.Name, newAttackType);
        //넉백효과
        Knockback(other, knockbackForce);
    }

    private void Knockback(Collider other, float knockbackForce)
    {
        WeaponType weaponType = PlayerHitWhiteBox.WhiteBox.weaponType;
        switch (weaponType)
        {
            case WeaponType.Hammer:
                hitDirection = other.GetComponentInParent<PlayerMove>().gameObject.transform.position - transform.position;
                break;
            case WeaponType.Rope:
                hitDirection = other.gameObject.transform.position - transform.position;
                break;
        }

        Vector3 knockbackDir = -hitDirection.normalized;

        rb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Debug.Log("피격자에게 넉백 적용: " + knockbackDir);
    }
}
