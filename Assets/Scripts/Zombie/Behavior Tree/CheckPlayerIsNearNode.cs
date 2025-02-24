using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerIsNearNode : Node
{
    private Transform transform;
    private int playerLayer = 1 << 6;
    private Animator anim;

    public CheckPlayerIsNearNode(Transform transform, Animator anim)
    {
        this.transform = transform;
        this.anim = anim;
    }
    public override E_NodeState Evaluate()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, 10f, playerLayer);
        if(collider.Length <= 0)
        {
            Debug.Log("발견");
            anim.SetFloat("speed", 0);
            return curState = E_NodeState.Failure;
        }

        // TODO : 애니메이션 추가
        return curState = E_NodeState.Success;
    }
}
