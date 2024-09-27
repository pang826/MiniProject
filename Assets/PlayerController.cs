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
    
    Vector3 dir;
    Vector3 mouseDir;
    Vector3 mousePos;

    [Header("�Ӽ�")]
    [SerializeField] Animator animator;
    [SerializeField] float curSpeedType;
    [SerializeField] float moveSpeed = 5f;   // �⺻ ������ �ӵ�
    [SerializeField] float runSpeed = 8f;    // ���� �ӵ�
    [SerializeField] float aimSpeed = 2.5f;  // ���ؽ� ������ �ӵ�
    
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
        // ���¿� ���� �̵��ӵ� ����
        // ���� ���º��� �ִϸ��̼� �ٸ��� ������ ����
        switch (curState)
        {
            case State.idle:
                animator.Play("Idle");
                Idle();
                break;
            case State.walk:
                animator.Play("Walk With Weapon");
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

    // �ǰ�
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

    // �ǰ� �����ð�
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
    // Ű �Է¹ޱ�
    void InputMoveKey()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mouseDir = Input.mousePosition;                                         // input.mousePosition ���� �޾ƿ� ���� 2D ��ũ�� ��ǥ�� z ���� ���ٰ� ��.
        mouseDir.z = Camera.main.transform.position.y - transform.position.y;   // z�� = ī�޶��� ���� = ī�޶��� ���� �� - player�� ����
        mousePos = Camera.main.ScreenToWorldPoint(mouseDir);                    // ��ũ����ǥ���� ������ǥ������ ����
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
        // ���Ŀ� �ٶ󺸴� ���⿡ ���� �ִϸ��̼� ��ȭ �ʿ�
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        if(Input.GetKey(KeyCode.W))
        {
            animator.Play("Aim Walk Forward");
        }
        else if(Input.GetKey(KeyCode.S))
        {
            animator.Play("Aim Walk Back");
        }
        else if(Input.GetKey(KeyCode.A))
        {
            animator.Play("Aim Walk Left");
        }
        else if(Input.GetKey(KeyCode.D))
        {
            animator.Play("Aim Walk Right");
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        if(Input.GetMouseButtonUp(1))
        {
            curState = pastState;
        }
        if(Input.GetMouseButtonDown(0))
        {
            animator.Play("AttackMotion");
        }
    }

    void AimMove()
    {
        curSpeedType = aimSpeed;
        rigid.MovePosition(transform.position + dir * curSpeedType * Time.deltaTime);
    }
}
