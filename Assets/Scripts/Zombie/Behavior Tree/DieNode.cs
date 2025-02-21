using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : Node
{
    private Transform transform;

    private Animator anim;

    private ZombieBT bt;

    private bool isDieAnimPlay;
    public DieNode(Transform transform, Animator anim, ZombieBT bt)
    {
        this.transform = transform;
        this.anim = anim;
        this.bt = bt;
    }

    public override E_NodeState Evaluate()
    {
        if(bt.Hp <= 0)
        {
            if(isDieAnimPlay == false)
            {
                GameManager.Instance.DeadZombie();
                isDieAnimPlay = true;
                anim.SetTrigger("death");
            }
            return E_NodeState.Success;
        }
        return E_NodeState.Failure;    
    }
}
