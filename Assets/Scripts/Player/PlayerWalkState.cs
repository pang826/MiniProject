using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : IPlayerState
{
    private PlayerController controller;
    private Animator anim;
    private Vector3 dir;
    private Rigidbody rigid;
    private float speed;
    public PlayerWalkState(PlayerController controller, Animator anim, float speed, Rigidbody rigid)
    {
        this.controller = controller;
        this.anim = anim;
        this.speed = speed;
        this.rigid = rigid;
    }

    public void OnEnter()
    {
        anim.SetBool("isWalking", true);
    }

    public void OnUpdate()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if(dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, lookRotation, 360 * Time.deltaTime);
        }

        rigid.MovePosition(controller.transform.position + (controller.transform.forward * dir.sqrMagnitude).normalized * Time.deltaTime * speed);

        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            controller.ChangeState(E_PlayerState.Idle);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            controller.ChangeState(E_PlayerState.Run);
        }
    }

    public void OnExit()
    {
        anim.SetBool("isWalking", false);
    }
}
