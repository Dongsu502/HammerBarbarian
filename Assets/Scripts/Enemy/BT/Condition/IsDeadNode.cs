using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDeadNode : BTNode
{
    private IMonster monster;
    public IsDeadNode(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        if(monster.HP <= 0)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
