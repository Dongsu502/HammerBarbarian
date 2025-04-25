using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : BTNode
{
    private IMonster monster;

    public AttackAction(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        monster.Attack();
        return NodeState.SUCCESS;
    }
}
