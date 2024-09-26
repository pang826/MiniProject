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

    [Header("속성")]
    [SerializeField] float moveSpeed = 5f;   // 기본 움직임 속도
    [SerializeField] float runSpeed = 8f;    // 질주 속도
    [SerializeField] float aimSpeed = 2.5f;    // 조준시 움직임 속도
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

        // 상태에 따른 이동속도 차이
        // 추후 상태별로 애니메이션 다르게 설정할 예정
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
    // 키 입력받기
    void InputMoveKey()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mouseDir = Input.mousePosition;                                         // input.mousePosition 으로 받아온 값은 2D 스크린 좌표라서 z 값이 없다고 함.
        mouseDir.z = Camera.main.transform.position.y - transform.position.y;   // z값 = 카메라의 깊이 = 카메라의 높이 값 - player의 높이
        mousePos = Camera.main.ScreenToWorldPoint(mouseDir);                    // 스크린좌표값을 월드좌표값으로 변경

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

    // 캐릭터 회전
    void Look()
    {
        // 우클릭을 하고 있을 때(조준) 마우스 커서 방향으로 회전
        if(curState == State.aim) 
        {
            transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        }
        // 캐릭터 회전
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
    }

    // 캐릭터 이동 (상태별로 속도 상이)
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
