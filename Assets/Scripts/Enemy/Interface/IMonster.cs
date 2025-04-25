using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster
{
    int HP { get; }
    bool IsHit { get; }
    bool TargetDetected { get; }
    bool InAttackRange { get; }

    void Death();
    void Hit();
    void Attack();
    void MoveToTarget();
    void Idle();
}
