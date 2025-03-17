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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f, playerLayer);
        
        if (colliders.Length >= 1)
        {
            foreach (Collider collider in colliders) 
            {
                if(collider.isTrigger == false)
                {
                    Debug.Log(colliders[0]);
                    Debug.Log("발견");
                    anim.SetFloat("speed", 0);
                    return E_NodeState.Success;
                }
            }
        }
        return curState = E_NodeState.Failure;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 색상 지정
        Gizmos.DrawWireSphere(transform.position, 0.01f); // 구 영역 시각화
    }
}
