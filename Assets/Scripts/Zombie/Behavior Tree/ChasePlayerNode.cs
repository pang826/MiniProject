using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerNode : Node
{
    private Transform player;
    private Transform transform;
    private Animator anim;
    private float speed;
    private bool isAttack;
    private ZombieBT bt;

    public ChasePlayerNode(Transform player, Transform transform, Animator anim, float speed, bool isAttack, ZombieBT bt)
    {
        this.player = player;
        this.transform = transform;
        this.anim = anim;
        this.speed = speed;
        this.isAttack = isAttack;
        this.bt = bt;
    }
    public override E_NodeState Evaluate()
    {
        if(bt.IsAttack == false && bt.IsStuck == false) 
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
        }
        anim.SetFloat("speed", 0.5f);
        return curState = E_NodeState.Running;
    }
}
