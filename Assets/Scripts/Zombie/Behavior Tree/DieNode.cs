using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieNode : Node
{
    private Transform transform;

    private Animator anim;

    private int hp;

    private bool isDieAnimPlay;
    public DieNode(Transform transform, Animator anim, int hp)
    {
        this.transform = transform;
        this.anim = anim;
        this.hp = hp;
    }

    public override E_NodeState Evaluate()
    {
        //if( 체력이 0이라면 success)
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            hp -= 3;
        }

        if(hp <= 0)
        {
            if(isDieAnimPlay == false)
            {
                isDieAnimPlay = true;
                anim.SetTrigger("death");
            }
            return E_NodeState.Success;
        }
        return E_NodeState.Failure;    
    }
}
