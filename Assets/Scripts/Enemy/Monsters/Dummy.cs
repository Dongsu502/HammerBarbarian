using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IMonster
{
    public string Name { get; private set; } = "Dummy";
    public int HP { get; private set; } = 100;
    public bool IsHit { get; set; }
    public bool IsBeingHit { get; private set; }
    public bool TargetDetected { get; set; }
    public bool InAttackRange { get; set; }
    public bool IsAttacking { get; private set; }

    private MonsterHitBox hitBoxClass;
    private Animator animator;

    private void Awake()
    {
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();
        animator = GetComponent<Animator>();
    }

    public void ResetBeingHit()
    {
        IsBeingHit = false;
    }

    private void HitAnimation()
    {
        if (!hitBoxClass.IsKnockback)
        {
            //약공격 히트 애니메이션
            animator.SetFloat("Attack_Light", 0);
            Debug.LogError("약공격 히트");
        }
        else
        {
            //강공격 히트 애니메이션
            animator.SetFloat("Attack_Heavy", 1);
            Debug.LogError("강공격 히트");

            hitBoxClass.IsKnockback = false;
        }

        
    }

    public void Death()
    {
        return;
    }
    public void Hit()
    {
        IsHit = false;
        IsBeingHit = true;
        HitAnimation();
    }
    public void Attack()
    {
        Idle();
    }
    public void MoveToTarget()
    {
        Idle();
    }
    public void Idle()
    {
        return;
    }
}
