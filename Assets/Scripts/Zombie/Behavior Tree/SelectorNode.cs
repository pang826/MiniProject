using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : Node
{
    public SelectorNode() : base() { }

    public SelectorNode(List<Node> children) : base(children) { }

    public override E_NodeState Evaluate()
    {
        foreach (Node child in children) 
        {
            switch(child.Evaluate()) 
            {
                case E_NodeState.Failure:
                    continue;
                case E_NodeState.Success:
                    return curState = E_NodeState.Success;
                case E_NodeState.Running:
                    return curState = E_NodeState.Running;
                default:
                    break;
            }
        }
        return curState = E_NodeState.Failure;
    }
}
