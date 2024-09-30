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
        Debug.Log($"{isDamaged} �ǰ�");
        Debug.Log($"{isAttack} ����");
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
        else if (Vector3.Distance(transform.position, target.transform.position) <= data.AttackRange)
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
        if (Vector3.Distance(transform.position, target.transform.position) > data.AttackRange)
        {
            curState = State.trace;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) <= data.AttackRange)
        {
            if (isAttack == false && isDamaged == false)
            {
                StartCoroutine(AttackCoroutine());
                // ���� ���� ����
                attackArea.enabled = true;
            }
            else if (isDamaged)
            {
                // ���� ���� ����
                attackArea.enabled = false;
            }
        }
        if (curHp <= 0)
        {
            StartCoroutine("DeathAnim");
            curState = State.die;
        }
    }

    IEnumerator AttackCoroutine()
    {
        // ���� ��� ����
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(1f);
        // ���� ���� ��� �ð� ����
        isAttack = true;
        

        yield return new WaitForSeconds(0.2f);
        // �ִϸ��̼� ��
        anim.SetBool("isAttack", false);
        
        yield return new WaitForSeconds(1f);

        // ���� ���� ��� �ð� ��
        isAttack = false;
        yield return new WaitForSeconds(3f);

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
