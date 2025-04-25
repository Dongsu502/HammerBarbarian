using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : BTNode
{
    private List<BTNode> children;

    public Sequence(List<BTNode> children)
    {
        this.children = children;
    }

    /// <summary>
    /// 자식 모두 성공해야 성공
    /// </summary>
    /// <returns></returns>
    public override NodeState Evaluate()
    {
        foreach(BTNode node in children)
        {
            var result = node.Evaluate();
            if (result == NodeState.FAILURE) return NodeState.FAILURE;
            if (result == NodeState.RUNNING) return NodeState.RUNNING;
        }
        return NodeState.SUCCESS;
    }
}
