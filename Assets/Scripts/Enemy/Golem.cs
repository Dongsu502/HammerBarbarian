using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour, IMonster
{
    public int HP { get; private set; } = 100;
    public bool IsHit { get; set; }
    public bool TargetDetected { get; set; }
    public bool InAttackRange { get; set; }

#if UNITY_EDITOR
    [ContextMenu("Die")]
    public void DieTest()
    {
        HP = 0;
    }
    [ContextMenu("Hit")]
    public void HitTest()
    {
        IsHit = true;
    }
    [ContextMenu("Attack")]
    public void AttackTest()
    {
        InAttackRange = true;
    }
    [ContextMenu("Detect")]
    public void MoveToTargetTest()
    {
        TargetDetected = true;
    }
#endif

    public void Death()
    {
        //사망 애니메이션 플레이
        //콜라이더 제거
        //리지드바디 IsKinematic: true
        Debug.Log("골렘 사망");
    }
    public void Hit()
    {
        //피격 애니메이션 플레이
        Debug.Log("골렘 피격");
    }
    public void Attack()
    {
        //공격 애니메이션 플레이
        Debug.Log("골렘 공격");
    }
    public void MoveToTarget()
    {
        //걷기 애니메이션 플레이
        Debug.Log("골렘 접근중..");
    }
    public void Idle()
    {
        //Idle 애니메이션 플레이
        Debug.Log("골렘 대기중");
    }
}
