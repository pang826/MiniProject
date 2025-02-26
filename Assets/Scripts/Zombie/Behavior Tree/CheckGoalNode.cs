using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGoalNode : Node
{
    private Transform transform;

    private Animator anim;

    private ZombieBT bt;

    private Transform goal;
    public CheckGoalNode(Transform transform, Animator anim, ZombieBT bt, Transform goal)
    {
        this.transform = transform;
        this.anim = anim;
        this.bt = bt;
        this.goal = goal;
    }
    public override E_NodeState Evaluate()
    {
        if(Vector3.Distance(transform.position, goal.position) <= 2f)
        {
            return curState = E_NodeState.Success;
        }
        return curState = E_NodeState.Failure;
    }
}
