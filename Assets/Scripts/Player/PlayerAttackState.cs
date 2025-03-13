using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    PlayerController controller;
    Animator anim;
    bool isContackAim;
    ZombieBT contactZombie;
    ZombieBT tartgetZombie;
    Transform lastZombie;
    public PlayerAttackState(PlayerController controller, Animator anim)
    {
        this.controller = controller;
        this.anim = anim;
    }

    public void OnEnter()
    {
        anim.SetBool("isAiming", true);
    }

    public void OnUpdate()
    {
        // 마우스 위치 받아와서 캐릭터가 거기로 회전하도록 설정
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y - controller.transform.position.y; // 높이 보정
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 direction = (worldMousePos - controller.transform.position).normalized;
        direction.y = 0;

        Vector3 shootOrigin = controller.transform.position + new Vector3(0, 0.5f, 0);
        RaycastHit hit;
        // Ray에 타겟이 닿을 경우
        if (Physics.Raycast(shootOrigin, direction, out hit, 100))
        {
            
            contactZombie = hit.collider.GetComponent<ZombieBT>();
            if (contactZombie != null)
            {
                if(tartgetZombie != contactZombie && tartgetZombie != null)
                {
                    Debug.Log("다른 타겟");
                    tartgetZombie.gameObject.GetComponent<Outline>().enabled = false;
                    tartgetZombie = null;
                    lastZombie = null;
                    //isContackAim = false;
                }
                contactZombie.gameObject.GetComponent<Outline>().enabled = true;
                tartgetZombie = contactZombie;
                lastZombie = hit.transform;
                //isContackAim = true;

            }
        }
        // Ray에 타겟이 닿지 않을 경우
        else
        {
            Debug.Log("타겟이 닿지 않음");
            if (tartgetZombie != null)
            {
                tartgetZombie.gameObject.GetComponent<Outline>().enabled = false;
                //isContackAim = false;
                tartgetZombie = null;
                lastZombie = null;
            }
        }

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            controller.transform.rotation = Quaternion.RotateTowards(controller.transform.rotation, lookRotation, 360 * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(direction);
        }


        if (Input.GetMouseButtonUp(1))
        {
            controller.ChangeState(E_PlayerState.Idle);
        }
    }

    public void OnExit()
    {
        anim.SetBool("isAiming", false);
    }

    private void Shoot(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 shootOrigin = controller.transform.position + new Vector3(0, 0.5f, 0);

        if (Physics.Raycast(shootOrigin, direction, out hit, 100))
        {
            ZombieBT zombie = hit.collider.GetComponent<ZombieBT>();
            if (zombie != null)
            {
                zombie.Damaged();
                Debug.Log("명중");
            }
        }
    }

    private void AimContactZombie(Vector3 shootOrigin, Vector3 direction)
    {
        
    }
}
