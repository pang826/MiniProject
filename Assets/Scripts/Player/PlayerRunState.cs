using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IPlayerState
{
    PlayerController controller;
    Animator anim;
    Vector3 dir;
    Rigidbody rigid;
    float speed;
    public PlayerRunState(PlayerController controller, Animator anim, Rigidbody rigid, float speed)
    {
        this.controller = controller;
        this.anim = anim;
        this.rigid = rigid;
        this.speed = speed;
    }

    public void OnEnter()
    {
        anim.SetBool("isRunning", true);
    }

    public void OnUpdate()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, lookRotation, 360 * Time.deltaTime);
        }

        rigid.MovePosition(controller.transform.position + (controller.transform.forward * dir.sqrMagnitude).normalized * Time.deltaTime * speed);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            controller.ChangeState(E_PlayerState.Idle);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            controller.ChangeState(E_PlayerState.Walk);
        }
    }

    public void OnExit()
    {
        anim.SetBool("isRunning", false);
    }
}
