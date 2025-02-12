using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerNode : Node
{
    private Transform player;
    private Transform transform;
    private bool isAttack;
    private float curTime = 0f;
    private float attackCooldown = 3f;
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
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= 1)
        {
            if (!isAttack)
            {
                isAttack = true;
                curTime = 0f;
                Attack();  // 공격 실행
                return E_NodeState.Running;  // 공격 중 상태 유지
            }
        }

        if (isAttack)
        {
            curTime += Time.deltaTime;
            if (curTime >= attackCooldown) // 쿨타임이 지나면 다시 공격 가능
            {
                isAttack = false;
                return E_NodeState.Success;
            }
            return E_NodeState.Running;  // 쿨타임 대기 중
        }

        return E_NodeState.Failure;
    }

    private void Attack()
    {
        Debug.Log("공격 실행!");
        // 실제 공격 로직 (애니메이션 재생, 데미지 적용 등) 추가 가능
        bt.StartAttackRoutine();
        anim.SetTrigger("isAttack");
    }
}
