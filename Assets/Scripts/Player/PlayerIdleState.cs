using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IPlayerState
{
    PlayerController controller;
    Animator anim;
    public PlayerIdleState(PlayerController controller, Animator anim)
    {
        this.controller = controller;
        this.anim = anim;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            controller.ChangeState(E_PlayerState.Walk);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.ChangeState(E_PlayerState.Run);
        }
    }

    public void OnExit() 
    {

    }
}
