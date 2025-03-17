using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class GoToCenterNode : Node
{
    private Transform transform;

    private Transform goal;

    private Animator anim;

    private ZombieBT bt;

    private float speed;

    public GoToCenterNode(Transform transform, Transform goal, Animator anim, ZombieBT bt, float speed)
    {
        this.transform = transform;
        this.goal = goal;
        this.anim = anim;
        this.bt = bt;
        this.speed = speed;
    }
    public override E_NodeState Evaluate()
    {
        transform.LookAt(goal);
        transform.position = Vector3.MoveTowards(transform.position, goal.position, Time.deltaTime * speed);
        anim.SetFloat("speed", 0.5f);
        return curState = E_NodeState.Running;
    }
}
