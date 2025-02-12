using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedNode : Node
{
    Animator anim;
    bool isDamaged;
    bool isCheck;
    public DamagedNode(Animator anim, bool isDamaged)
    {
        this.anim = anim;
        this.isDamaged = isDamaged;
    }
    public override E_NodeState Evaluate()
    {
        if(isDamaged)
        {
            anim.SetBool("isDamage", true);
            isCheck = true;
            return E_NodeState.Running;
        }

        if(isDamaged == false && isCheck == true)
        {
            isCheck = false;
            anim.SetBool("isDamage", false);
        }
        return curState = E_NodeState.Failure;
    }
}
