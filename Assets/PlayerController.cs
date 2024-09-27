using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { idle, walk, run, aim }
    State curState;
    State pastState;

    MeshRenderer[] meshs;
    Rigidbody rigid;
    [SerializeField] Animator animator;
    Vector3 dir;
    Vector3 mouseDir;
    Vector3 mousePos;

    [Header("속성")]
    [SerializeField] float curSpeedType;
    [SerializeField] float moveSpeed = 5f;   // 기본 움직임 속도
    [SerializeField] float runSpeed = 8f;    // 질주 속도
    [SerializeField] float aimSpeed = 2.5f;  // 조준시 움직임 속도
    
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] int hp;
    bool isDamaged;
    private void Awake()
    {
        hp = 30;
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InputMoveKey();
        if (pastState != curState && curState != State.aim)
        {
            pastState = curState;
        }
        // 상태에 따른 이동속도 차이
        // 추후 상태별로 애니메이션 다르게 설정할 예정
        switch (curState)
        {
            case State.idle:
                animator.Play("Standing Idle");
                Idle();
                break;
            case State.walk:
                animator.Play("Walk With Briefcase");
                Walk();
                break;
            case State.run:
                animator.Play("Run");
                Run();
                break;
            case State.aim:
                Aim();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (curState)
        {
            case State.walk:
                WalkMove();
                break;
            case State.run:
                RunMove();
                break;
            case State.aim:
                AimMove();
                break;
        }
    }

    // 피격
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster")
        {
            Debug.Log(collision.gameObject.name);
            if (!isDamaged)
            {
                MonsterController monster = collision.collider.GetComponent<MonsterController>();
                hp -= monster.dmg;
                StartCoroutine(OnDamage());
            }
        }
    }

    // 피격 무적시간
    IEnumerator OnDamage()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);

        isDamaged = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        
        yield return delay;
        isDamaged = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }
    // 키 입력받기
    void InputMoveKey()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mouseDir = Input.mousePosition;                                         // input.mousePosition 으로 받아온 값은 2D 스크린 좌표라서 z 값이 없다고 함.
        mouseDir.z = Camera.main.transform.position.y - transform.position.y;   // z값 = 카메라의 깊이 = 카메라의 높이 값 - player의 높이
        mousePos = Camera.main.ScreenToWorldPoint(mouseDir);                    // 스크린좌표값을 월드좌표값으로 변경
        if(Input.anyKey == false)
        {
            curState = State.idle;
        }
    }

    void Idle()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            curState = State.walk;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        else if (Input.GetMouseButton(1))
        {
            curState = State.aim;
        }
    }
    
    void Walk()
    {
        curSpeedType = moveSpeed;
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        else if (Input.GetMouseButton(1))
        {
            curState = State.aim;
        }
    }

    void WalkMove()
    {
        rigid.MovePosition(transform.position + (transform.forward * dir.sqrMagnitude).normalized * curSpeedType * Time.deltaTime);
    }

    void Run()
    {
        curSpeedType = runSpeed;
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && curState != State.aim)
        {
            curState = State.walk;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift) && Input.anyKey == false)
        {
            curState = State.idle;
        }
        if (Input.GetMouseButton(1))
        {
            curState = State.aim;
        }

    }

    void RunMove()
    {
        rigid.MovePosition(transform.position + (transform.forward * dir.sqrMagnitude).normalized * curSpeedType * Time.deltaTime);
    }

    void Aim()
    {
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        if(Input.GetMouseButtonUp(1))
        {
            curState = pastState;
        }
    }

    void AimMove()
    {
        curSpeedType = aimSpeed;
        rigid.MovePosition(transform.position + dir * curSpeedType * Time.deltaTime);
    }
}
