using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BTNode
{
    private List<BTNode> children;

    public Selector(List<BTNode> children)
    {
        this.children = children;
    }

    /// <summary>
    /// 자식 중 하나라도 성공하면 성공
    /// </summary>
    /// <returns></returns>
    public override NodeState Evaluate()
    {
        foreach(BTNode node in children)
        {
            var result = node.Evaluate();
            if (result == NodeState.SUCCESS || result == NodeState.RUNNING) return result;
        }
        return NodeState.FAILURE;
    }
}
