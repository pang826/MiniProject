using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedNode : Node
{
    Animator anim;
    ZombieBT bt;
    
    public DamagedNode(Animator anim, ZombieBT bt)
    {
        this.anim = anim;
        this.bt = bt;
    }
    public override E_NodeState Evaluate()
    {
        if(bt.IsDamaged)
        {
            Debug.Log("µ¥¹ÌÁö");
            anim.SetBool("isDamage", true);
            return E_NodeState.Running;
        }

        if(bt.IsDamaged == false)
        {
            anim.SetBool("isDamage", false);
        }
        return curState = E_NodeState.Failure;
    }
}
