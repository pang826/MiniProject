using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tree : MonoBehaviour
{
    private Node rootNode;
    // Start is called before the first frame update
    protected void Start()
    {
        rootNode = SetUpBehaviorTree();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (rootNode is null)
            return;
            
        rootNode.Evaluate();
    }

    protected abstract Node SetUpBehaviorTree();
}
