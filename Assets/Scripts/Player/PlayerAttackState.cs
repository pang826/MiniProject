using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    PlayerController controller;
    Animator anim;

    Vector3 mouseDir;

    BoxCollider collider;

    private float curTime;
    private float limitTime = 1.5f;
    private bool isAttacking;
    public PlayerAttackState(PlayerController controller, Animator anim, BoxCollider collider)
    {
        this.controller = controller;
        this.anim = anim;
        this.collider = collider;
    }

    public void OnEnter()
    {
        curTime = 0f;
    }

    public void OnUpdate()
    {
        Debug.Log($"현재시간{curTime}");
        Debug.Log($"제한시간{limitTime}");
        Debug.Log($"확인{isAttacking}");
 
        if (Input.GetMouseButton(1))
        {
            // 마우스 위치 받아와서 거기 쳐다보게 하기
            if (Input.GetMouseButtonDown(0) && isAttacking == false)
            {
                controller.StartAttackRoutine(curTime, limitTime, isAttacking, collider);
            }
        }
        else
        {
            if (isAttacking == false)
            {
                controller.ChangeState(E_PlayerState.Idle);
            }
        }
    }

    public void OnExit()
    {

    }

    
}
