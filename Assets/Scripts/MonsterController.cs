using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public MonsterData data;
    public enum State { idle, patrol, trace, attack, die }
    public State curState;
    [SerializeField] GameObject target;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] GameObject melee;
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider attackArea;
    Coroutine attackCoroutine;
    [Header("SFX")]


    [Header("속성")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] int curHp;
    bool isDamaged;
    bool isAttack;
    private void Awake()
    {
        isDamaged = false;
        isAttack = false;
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        anim = GetComponent<Animator>();
        attackArea = GetComponent<BoxCollider>();
        attackArea.enabled = false;
        curState = State.idle;
    }
    private void Start()
    {
        curHp = data.Hp;
        nav.speed = data.Speed;
        target = GameObject.FindGameObjectWithTag("Player");
        melee = GameObject.FindGameObjectWithTag("Melee");
    }

    private void Update()
    {
        Debug.Log(attackArea.gameObject.activeSelf);
        switch (curState)
        {
            case State.idle:
                Idle();
                break;
            case State.trace:
                Trace();
                break;
            case State.attack:
                Attack();
                break;
            case State.die:
                DIe();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Melee meleee = melee.GetComponent<Melee>();
            if (!isDamaged)
            {
                curHp -= meleee.dmg;
                StartCoroutine(OnDamage());
            }
            if (isDamaged)
            {
                // 경직
                nav.SetDestination(transform.position);
            }
        }
    }
    IEnumerator OnDamage()
    {
        // 무적시간 시작(데미지를 받았는지에 대한 여부)
        isDamaged = true;
        // 피격 애니메이션 시작
        anim.SetBool("isDamage", true);
        // 무적시간
        yield return new WaitForSeconds(0.1f);

        // 피격 애니메이션 끝
        anim.SetBool("isDamage", false);
        yield return new WaitForSeconds(0.5f);
        // 무적시간 끝
        isDamaged = false;
        yield break;
    }
    void Idle()
    {
        anim.SetFloat("speed", 0);
        nav.SetDestination(transform.position);

        if (Vector3.Distance(transform.position, target.transform.position) <= data.DetectDist)
        {
            curState = State.trace;
        }
        if (curHp <= 0)
        {
            StartCoroutine("DeathAnim");
            curState = State.die;
        }
    }

    void Trace()
    {
        anim.SetFloat("speed", data.Speed);
        nav.SetDestination(target.transform.position);
        if (Vector3.Distance(transform.position, target.transform.position) >= data.DetectDist + 5)
        {
            curState = State.idle;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= data.AttackRange - 0.1f)
        {
            curState = State.attack;
        }
        if (curHp <= 0)
        {
            StartCoroutine("DeathAnim");
            curState = State.die;
        }
    }

    void Attack()
    {
        anim.SetFloat("speed", 0);
        if (Vector3.Distance(transform.position, target.transform.position) <= data.AttackRange)
        {
            nav.SetDestination(transform.position);
            
            // 몬스터가 공격 대기시간이 아니고 피격 당한상태가 아닐 경우 공격
            if (isDamaged)
            {
                // 공격당한 상태일 경우 패스
                return;
            }
            else if (isAttack == false && isDamaged == false)
            {
                
                attackCoroutine = StartCoroutine(AttackCoroutine());
                attackArea.enabled = false;
            }
        }
        if (Vector3.Distance(transform.position, target.transform.position) > data.AttackRange + 0.1f)
        {
            curState = State.trace;
        }
        if (curHp <= 0)
        {
            StartCoroutine("DeathAnim");
            curState = State.die;
        }
    }

    IEnumerator AttackCoroutine()
    {
        // 공격 재사용 대기 시간 시작
        isAttack = true;
        // 공격 모션 시작
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(1.1f);
        // 공격 범위 생성
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
        // 애니메이션 끝
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(2f);
        // 공격 재사용 대기 시간 끝
        isAttack = false;

        yield break;
    }

    void DIe()
    {
        // 위치고정
        nav.SetDestination(transform.position);
    }

    IEnumerator DeathAnim()
    {
        // 피격 시 땅이외에는 충돌하지 않게 설정
        gameObject.layer = 9;
        // 죽음 애니메이션 시작
        anim.SetTrigger("death");
        // 5초후 삭제
        Destroy(gameObject, 5);
        yield break;
    }
}
