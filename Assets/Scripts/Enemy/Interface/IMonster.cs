using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster
{
    string Name { get; }
    int HP { get; }
    bool IsHit { get; set; }
    bool TargetDetected { get; set; }
    bool InAttackRange { get; set; }
    bool IsAttacking { get; }

    void Death();
    void Hit();
    void Attack();
    void MoveToTarget();
    void Idle();
}
