using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : IPlayerState
{
    PlayerController controller;
    Animator anim;
    public PlayerDieState(PlayerController controller, Animator anim)
    {
        this.controller = controller;
        this.anim = anim;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
