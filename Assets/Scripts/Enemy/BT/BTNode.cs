using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public abstract class BTNode
{
    public abstract NodeState Evaluate();
}
