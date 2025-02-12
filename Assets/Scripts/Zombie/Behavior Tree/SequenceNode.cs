using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

public class SequenceNode : Node
{
    public SequenceNode() : base() { }

    public SequenceNode(List<Node> children) : base(children) { }

    public override E_NodeState Evaluate()
    {
        bool nowRunning = false;
        foreach (Node child in children) 
        {
            switch(child.Evaluate())
            {
                case E_NodeState.Running:
                    nowRunning = true;
                    continue;
                case E_NodeState.Success:
                    continue;
                case E_NodeState.Failure:
                    return curState = E_NodeState.Failure;
                default:
                    continue;
            }
        }

        return curState = nowRunning? E_NodeState.Running : E_NodeState.Success;
    }
}
