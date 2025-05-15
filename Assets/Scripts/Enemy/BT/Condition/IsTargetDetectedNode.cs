using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetDetectedNode : BTNode
{
    private IMonster monster;
    public IsTargetDetectedNode(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        if(monster.TargetDetected && !monster.IsAttacking && !monster.IsBeingHit && monster.HP > 0)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
