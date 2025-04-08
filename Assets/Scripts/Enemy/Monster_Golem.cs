using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_Golem : Monster
{
    protected MonsterAttackDetection attackDetection;
    protected BoxCollider attackCollider;

    #region UnityCallFunc

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        attackDetection = GetComponentInChildren<MonsterAttackDetection>();
        attackCollider = attackDetection.attackCollider;
        attackCollider.gameObject.SetActive(false);

        agent.avoidancePriority = Random.Range(30, 70);
    }
    
    #endregion

    #region IEnumerator Machine

    protected override IEnumerator MonsterStateMachine()
    {
        return base.MonsterStateMachine();
    }

    protected override IEnumerator IDLE()
    {
        return base.IDLE();
    }

    protected override IEnumerator CHASE()
    {
        return base.CHASE();
    }

    protected override IEnumerator ATTACK()
    {
        
        if(target == null)
        {
            attackCollider.gameObject.SetActive(false);
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            attackCollider.gameObject.SetActive(false);
        }

        attackCollider.gameObject.SetActive(true);

        return base.ATTACK();
    }

    protected override IEnumerator HIT()
    {
        return base.HIT();
    }

    protected override IEnumerator DIE()
    {
        return base.DIE();
    }

    #endregion

    #region MonsterFunc

    protected override void MoveAnimation(bool isMoving)
    {
        base.MoveAnimation(isMoving);
    }

    protected override void ChangeState(State _newState)
    {
        base.ChangeState(_newState);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void MonsterDie()
    {
        base.MonsterDie();
    }

    #endregion
}
