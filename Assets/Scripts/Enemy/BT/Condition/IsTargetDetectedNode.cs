using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetDetectedNode : BTNode
{
    private IMonster monster;
    public IsTargetDetectedNode(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        if(monster.TargetDetected)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
