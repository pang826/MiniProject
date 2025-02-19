using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : IPlayerState
{
    PlayerController controller;
    Animator anim;

    private float limitTime = 0.5f;
    private float curTime = 0;
    public PlayerDamagedState(PlayerController controller, Animator anim)
    {
        this.controller = controller;
        this.anim = anim;
    }

    public void OnEnter()
    {
        controller.Hp -= 2;
        anim.SetTrigger("IsDamaged");
        curTime = 0;
    }

    public void OnUpdate()
    {
        curTime += Time.deltaTime;
        if (curTime >= limitTime)
        {
            curTime = 0;
            controller.ChangeState(E_PlayerState.Idle);
        }
    }

    public void OnExit()
    {
    }
}
