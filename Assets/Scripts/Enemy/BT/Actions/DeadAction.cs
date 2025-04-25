using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAction : BTNode
{
    private IMonster monster;

    public DeadAction(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        monster.Death();
        return NodeState.SUCCESS;
    }
}
