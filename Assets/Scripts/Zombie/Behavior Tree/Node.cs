using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Node
{
    protected E_NodeState curState;

    public Node parentNode;

    protected List<Node> children = new List<Node>();

    public Node()
    {
        parentNode = null;
    }

    public Node(List<Node> children)
    {
        foreach (Node node in children) 
        {
            this.children.Add(node);
            node.parentNode = this;
        }
    }

    public abstract E_NodeState Evaluate();
}
