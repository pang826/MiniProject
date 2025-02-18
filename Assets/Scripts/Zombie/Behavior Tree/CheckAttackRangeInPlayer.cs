using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackRangeInPlayer : Node
{
    private int playerLayer = 1 << 6;
    private Transform player;
    private Transform transform;
    private Animator anim;
    private ZombieBT bt;
    private bool isAttack;
    public CheckAttackRangeInPlayer(Transform player, Transform transform, Animator anim, ZombieBT bt)
    {
        this.player = player;
        this.transform = transform;
        this.anim = anim;
        this.bt = bt;
    }

    public override E_NodeState Evaluate()
    {
        if(bt.IsAttack)
        {
            return curState = E_NodeState.Running;
        }
        Collider[] collider = Physics.OverlapSphere(transform.position, 0.8f, playerLayer);
        if (collider.Length >= 1)
        {
            anim.SetFloat("speed", 0);
            return E_NodeState.Success;
        }
        return curState = E_NodeState.Failure;
    }
}
