using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerNode : Node
{
    private Transform player;
    private Transform transform;
    private float curTime = 0f;
    private float attackCooldown = 2f;
    private Animator anim;
    private ZombieBT bt;

    public AttackPlayerNode(Transform player, Transform transform, Animator anim, ZombieBT bt)
    {
        this.player = player;
        this.transform = transform;
        this.anim = anim;
        this.bt = bt;
    }

    public override E_NodeState Evaluate()
    {
        if (bt.IsAttack == false && bt.IsStuck == false)
        {
            bt.IsAttack = true;
            curTime = 0f;
            Attack();  // 공격 실행
            return curState = E_NodeState.Running;  // 공격 중 상태 유지
        }      
        
        if (bt.IsAttack)
        {
            curTime += Time.deltaTime;
            if (curTime >= attackCooldown) // 쿨타임이 지나면 다시 공격 가능
            {
                bt.IsAttack = false;
                return curState = E_NodeState.Failure;
            }
        }
        
        return curState = E_NodeState.Running;
    }

    private void Attack()
    {
        Debug.Log("공격 실행!");
        // 실제 공격 로직 (애니메이션 재생, 데미지 적용 등) 추가 가능
        //bt.StartAttackRoutine();
        anim.SetTrigger("IsAttack");
    }
}
