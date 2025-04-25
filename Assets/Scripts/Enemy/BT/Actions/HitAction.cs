using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAction : BTNode
{
    private IMonster monster;

    public HitAction(IMonster monster) => this.monster = monster;

    public override NodeState Evaluate()
    {
        monster.Hit();
        return NodeState.SUCCESS;
    }
}
