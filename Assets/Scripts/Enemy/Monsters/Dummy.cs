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
    private Rigidbody rb;

    private void Awake()
    {
        hitBoxClass = GetComponentInChildren<MonsterHitBox>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
            animator.SetTrigger("Attack_Light");
        }
        else
        {
            //강공격 히트 애니메이션
            animator.SetTrigger("Attack_Heavy");

            hitBoxClass.IsKnockback = false;
        }
    }

    public void Death()
    {
        Idle();
    }
    public void Hit()
    {
        IsHit = false;
        IsBeingHit = true;
        rb.isKinematic = true;

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

    }
}
