using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { walk, run, aim }
    State curState;

    MeshRenderer mesh;
    Rigidbody rigid;
    Vector3 dir;
    Vector3 mouseDir;
    Vector3 mousePos;

    [Header("�Ӽ�")]
    [SerializeField] float moveSpeed = 5f;   // �⺻ ������ �ӵ�
    [SerializeField] float runSpeed = 8f;    // ���� �ӵ�
    [SerializeField] float aimSpeed = 2.5f;    // ���ؽ� ������ �ӵ�
    float curSpeedType;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] int hp;
    bool isDamaged;
    private void Awake()
    {
        hp = 30;
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        InputMoveKey();
        Look();

        // ���¿� ���� �̵��ӵ� ����
        // ���� ���º��� �ִϸ��̼� �ٸ��� ������ ����
        switch (curState)
        {
            case State.walk:
                curSpeedType = moveSpeed;
                break;
            case State.run:
                curSpeedType = runSpeed;
                break;
            case State.aim:
                curSpeedType = aimSpeed;
                break;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

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

    IEnumerator OnDamage()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);

        isDamaged = true;
        mesh.material.color = Color.red;
        yield return delay;
        isDamaged = false;
        mesh.material.color = Color.yellow;
    }
    // Ű �Է¹ޱ�
    void InputMoveKey()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mouseDir = Input.mousePosition;                                         // input.mousePosition ���� �޾ƿ� ���� 2D ��ũ�� ��ǥ�� z ���� ���ٰ� ��.
        mouseDir.z = Camera.main.transform.position.y - transform.position.y;   // z�� = ī�޶��� ���� = ī�޶��� ���� �� - player�� ����
        mousePos = Camera.main.ScreenToWorldPoint(mouseDir);                    // ��ũ����ǥ���� ������ǥ������ ����

        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        if (Input.GetMouseButton(1))
        {
            curState = State.aim;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(1))
        { curState = State.walk; }
    }

    // ĳ���� ȸ��
    void Look()
    {
        // ��Ŭ���� �ϰ� ���� ��(����) ���콺 Ŀ�� �������� ȸ��
        if(curState == State.aim) 
        {
            transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        }
        // ĳ���� ȸ��
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
    }

    // ĳ���� �̵� (���º��� �ӵ� ����)
    private void Move()
    {
        if (curState != State.aim)
        {
            rigid.MovePosition(transform.position + (transform.forward * dir.sqrMagnitude).normalized * curSpeedType * Time.deltaTime);
        }
        if (curState == State.aim)
        {
            
            Vector3 aimVerticalDir = transform.forward * Input.GetAxis("Vertical") * curSpeedType * Time.deltaTime;
            Vector3 aimHorizontalDir = transform.right * Input.GetAxis("Horizontal") * curSpeedType * Time.deltaTime;
            if(Input.GetAxis("Vertical") != 0)
            {
                rigid.MovePosition(new Vector3(transform.position.x + aimVerticalDir.x, transform.position.y, transform.position.z + aimVerticalDir.z));
            }
            if(Input.GetAxis("Horizontal") != 0)
            {
                rigid.MovePosition(new Vector3(transform.position.x + aimHorizontalDir.x, transform.position.y, transform.position.z + aimHorizontalDir.z));
            }

        }
    }

}
