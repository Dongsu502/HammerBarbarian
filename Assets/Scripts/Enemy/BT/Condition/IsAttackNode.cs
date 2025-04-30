using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackNode : BTNode
{
    private IMonster monster;
    public IsAttackNode(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        if(monster.InAttackRange && !monster.IsBeingHit)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
