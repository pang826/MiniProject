using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : IPlayerState
{
    PlayerController controller;
    Animator anim;
    public PlayerDamagedState(PlayerController controller, Animator anim)
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
