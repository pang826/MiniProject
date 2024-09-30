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


    [Header("�Ӽ�")]
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
                // ����
                nav.SetDestination(transform.position);
            }
        }
    }
    IEnumerator OnDamage()
    {
        // �����ð� ����(�������� �޾Ҵ����� ���� ����)
        isDamaged = true;
        // �ǰ� �ִϸ��̼� ����
        anim.SetBool("isDamage", true);
        // �����ð�
        yield return new WaitForSeconds(0.1f);

        // �ǰ� �ִϸ��̼� ��
        anim.SetBool("isDamage", false);
        yield return new WaitForSeconds(0.5f);
        // �����ð� ��
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
            
            // ���Ͱ� ���� ���ð��� �ƴϰ� �ǰ� ���ѻ��°� �ƴ� ��� ����
            if (isDamaged)
            {
                // ���ݴ��� ������ ��� �н�
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
        // ���� ���� ��� �ð� ����
        isAttack = true;
        // ���� ��� ����
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(1.1f);
        // ���� ���� ����
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
        // �ִϸ��̼� ��
        anim.SetBool("isAttack", false);
        yield return new WaitForSeconds(2f);
        // ���� ���� ��� �ð� ��
        isAttack = false;

        yield break;
    }

    void DIe()
    {
        // ��ġ����
        nav.SetDestination(transform.position);
    }

    IEnumerator DeathAnim()
    {
        // �ǰ� �� ���̿ܿ��� �浹���� �ʰ� ����
        gameObject.layer = 9;
        // ���� �ִϸ��̼� ����
        anim.SetTrigger("death");
        // 5���� ����
        Destroy(gameObject, 5);
        yield break;
    }
}
