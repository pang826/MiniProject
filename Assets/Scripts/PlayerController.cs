using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public enum State { idle, walk, run, aim, die }
    public State curState;
    State pastState;

    Vector3 dir;
    Vector3 mouseDir;
    Vector3 mousePos;

    [Header("참조")]
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rigid;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] Material[] materials;
    [SerializeField] Melee melee;
    [SerializeField] GameObject inGameUi;
    [SerializeField] GameObject dieUi;
    [SerializeField] TextMeshProUGUI curScoreBoard;
    [SerializeField] TextMeshProUGUI finalScoreBoard;

    [Header("SFX")]
    [SerializeField] AudioSource attackSFX;
    [SerializeField] AudioSource dieSFX;

    [Header("속성")]
    [SerializeField] float curSpeedType;
    [SerializeField] float moveSpeed = 5f;   // 기본 움직임 속도
    [SerializeField] float runSpeed = 8f;    // 질주 속도
    [SerializeField] float aimSpeed = 2.5f;  // 조준시 움직임 속도
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float attackDelay;      // 다시 공격하는데 걸리는 시간
    [SerializeField] int hp;
    bool canMove;   // 움직임 가능 여부
    bool isDamaged; // 피격 여부
    bool isAttack;  // 공격 여부
    bool isDie;     // 사망 여부
    float time;
    int score;
    private void Awake()
    {
        hp = 15;
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
        melee = GetComponentInChildren<Melee>();
        canMove = true;
        isDamaged = false;
        isAttack = false;
        isDie = false;
        anim.SetBool("isAttack", true);
    }
    private void Start()
    {
        inGameUi = GameObject.FindGameObjectWithTag("InGameUi");
        
        dieUi = GameObject.FindGameObjectWithTag("DieUi");
        dieUi.SetActive(false);
    }
    private void Update()
    {
        if(curState != State.die)
        {
            time += Time.deltaTime;
            score = (int)time;
            curScoreBoard.text = score.ToString();
        }
        if (curState == State.die)
        {
            score = (int)time;
            finalScoreBoard.text = score.ToString();
        }

            InputMoveKey();
        // 이전 상태를 담아두는 변수(조준상태에서 이전상태로 돌아가기 위함)
        if (pastState != curState && curState != State.aim)
        {
            pastState = curState;
        }
        // 상태에 따른 이동속도 차이
        // 추후 상태별로 애니메이션 다르게 설정할 예정
        switch (curState)
        {
            case State.idle:
                Idle();
                break;
            case State.walk:
                Walk();
                break;
            case State.run:
                Run();
                break;
            case State.aim:
                Aim();
                break;
            case State.die:
                Die();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
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
    }

    // 피격
    private void OnCollisionEnter(Collision collision)
    {
        MonsterController monster = collision.collider.GetComponent<MonsterController>();
        if (collision.collider.GetComponent<BoxCollider>() && isDamaged == false && monster.curState != MonsterController.State.die)
        {
            hp -= monster.data.Dmg;
            Debug.Log(hp);
            StartCoroutine(OnDamage());
        }
        else
        {
            return;
        }
    }

    // 피격 무적시간
    IEnumerator OnDamage()
    {
        isDamaged = true;
        anim.SetBool("isDamaged", true);
        
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isDamaged", false);
        yield return new WaitForSeconds(0.9f);
        
        isDamaged = false;
        yield break;
    }

    // 공격 모션
    IEnumerator AttackMotion()
    {
        canMove = false;
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(1f);
        
        anim.SetBool("isAttack", true);
        canMove = true;
        StopCoroutine("AttackMotion");
    }

    // 공격
    void Attack()
    {
        attackDelay += Time.deltaTime;
        if (melee.attackSpeed <= attackDelay)
        {
            isAttack = false;
        }
        if (!isAttack && Input.GetMouseButtonDown(0) && curState == State.aim)
        {
            melee.Attack();
            attackDelay = 0f;
            isAttack = true;

            SoundManager.Instance.PlaySFX(attackSFX);
            StartCoroutine(AttackMotion());
        }
    }
    // 키 입력받기
    void InputMoveKey()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }

        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        mouseDir = Input.mousePosition;                                         // input.mousePosition 으로 받아온 값은 2D 스크린 좌표라서 z 값이 없다고 함.
        mouseDir.z = Camera.main.transform.position.y - transform.position.y;   // z값 = 카메라의 깊이 = 카메라의 높이 값 - player의 높이
        mousePos = Camera.main.ScreenToWorldPoint(mouseDir);                    // 스크린좌표값을 월드좌표값으로 변경

    }

    void Idle()
    {
        if (hp <= 0)
        {
            curState = State.die;
        }
        anim.SetBool("isWalking", false);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            curState = State.walk;
        }
        else if (Input.GetMouseButton(1) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            curState = State.aim;
        }
    }

    void Walk()
    {
        if (hp <= 0)
        {
            curState = State.die;
        }
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", true);

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
        else if (Input.GetMouseButton(1) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            curState = State.aim;
        }
        else if (Input.anyKey == false)
        {
            curState = State.idle;
        }
    }

    void WalkMove()
    {
        if (anim.GetBool("isAttack") == true)
            rigid.MovePosition(transform.position + (transform.forward * dir.sqrMagnitude).normalized * curSpeedType * Time.deltaTime);
    }

    void Run()
    {
        if (hp <= 0)
        {
            curState = State.die;
        }
        if (rigid.velocity.z < 0.5f)
        {
            anim.SetBool("isRunning", true);
        }

        curSpeedType = runSpeed;
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && curState != State.aim)
        {
            curState = State.walk;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && Input.anyKey == false)
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
        if (hp <= 0)
        {
            curState = State.die;
        }
        anim.SetBool("isAiming", true);
        anim.SetBool("isWalking", false);
        anim.SetBool("isRunning", false);
        // 이후에 바라보는 방향에 맞춰 애니메이션 변화 필요
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));
        Attack();

        if (Input.GetAxisRaw("Horizontal") == 1)
        {
            anim.SetBool("AimRight", true);
        }
        else if (Input.GetAxisRaw("Horizontal") == -1)
        {
            anim.SetBool("AimLeft", true);
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("AimRight", false);
            anim.SetBool("AimLeft", false);
        }

        if (Input.GetAxisRaw("Vertical") == 1)
        {
            anim.SetBool("AimForward", true);
        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            anim.SetBool("AimBack", true);
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            anim.SetBool("AimForward", false);
            anim.SetBool("AimBack", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("AimRight", false);
            anim.SetBool("AimLeft", false);
            anim.SetBool("AimForward", false);
            anim.SetBool("AimBack", false);
            anim.SetBool("isAiming", false);
            curState = pastState;
        }
    }

    void AimMove()
    {
        curSpeedType = aimSpeed;
        if (anim.GetBool("isAttack") == true)
        {
            rigid.MovePosition(transform.position + dir * curSpeedType * Time.deltaTime);
        }
    }

    void Die()
    {
        if(!isDie)
        {
            // 피격 시 땅이외에는 충돌하지 않게 설정
            gameObject.layer = 10;
            // 우클릭 누른상태로 사망시 우클릭으로 할 수 애니메이션 취소
            anim.SetBool("isAiming", false);
            anim.SetBool("AimRight", false);
            anim.SetBool("AimLeft", false);
            anim.SetBool("AimForward", false);
            anim.SetBool("AimBack", false);
            // 죽음 애니메이션 시작
            anim.SetTrigger("Die");
            // BGM 조작
            SoundManager.Instance.PlaySFX(dieSFX);
            SoundManager.Instance.StopGameBgm();
            SoundManager.Instance.PlayDieBgm();
            // InGameUI 끄기
            inGameUi.SetActive(false);
            dieUi.SetActive(true);
            // 죽음상태 ON
            isDie = true;
        }
    }
}
