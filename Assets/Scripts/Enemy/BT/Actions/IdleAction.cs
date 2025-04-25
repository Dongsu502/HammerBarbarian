using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : BTNode
{
    private IMonster monster;

    public IdleAction(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        monster.Idle();
        return NodeState.SUCCESS;
    }
}
