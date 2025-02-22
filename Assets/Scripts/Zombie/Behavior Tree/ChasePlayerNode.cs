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

    public ChasePlayerNode(Transform player, Transform transform, Animator anim, float speed, bool isAttack)
    {
        this.player = player;
        this.transform = transform;
        this.anim = anim;
        this.speed = speed;
        this.isAttack = isAttack;
    }
    public override E_NodeState Evaluate()
    {
        if(isAttack == false) 
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);
        }
        Debug.Log("추적");
        //TODO : 애니메이션 추가
        anim.SetFloat("speed", 0.5f);
        return curState = E_NodeState.Running;
    }
}
