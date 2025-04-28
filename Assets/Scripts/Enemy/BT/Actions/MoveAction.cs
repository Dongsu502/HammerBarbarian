using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BTNode
{
    private IMonster monster;

    public MoveAction(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        monster.MoveToTarget();
        return NodeState.RUNNING;
    }
}
