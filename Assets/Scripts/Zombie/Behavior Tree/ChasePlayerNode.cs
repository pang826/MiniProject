using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerNode : Node
{
    private Transform player;
    private Transform transform;
    private Animator anim;
    private float speed;

    public ChasePlayerNode(Transform player, Transform transform, Animator anim, float speed)
    {
        this.player = player;
        this.transform = transform;
        this.anim = anim;
        this.speed = speed;
    }
    public override E_NodeState Evaluate()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * speed);

        //TODO : 애니메이션 추가
        anim.SetFloat("speed", 0.5f);
        return curState = E_NodeState.Running;
    }
}
