using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHitNode : BTNode
{
    private IMonster monster;
    public IsHitNode(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        if(monster.IsHit)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
