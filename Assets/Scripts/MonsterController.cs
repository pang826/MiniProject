using System.Collections;
using Unity.VisualScripting;
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

    [Header("�Ӽ�")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] int curHp;
    //public int dmg;
    //[SerializeField] float detectDist;
    //[SerializeField] float attackRange;
    //[SerializeField] float speed;
    bool isDamaged;
    private void Awake()
    {
        //speed = 5f;
        //dmg = 3;
        //detectDist = 30;
        //attackRange = 3;
        isDamaged = false;
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        meshs = GetComponentsInChildren<MeshRenderer>();
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
        if(other.tag == "Melee")
        {
            Melee meleee = melee.GetComponent<Melee>();
            if(!isDamaged)
            {
                curHp -= meleee.dmg;
                StartCoroutine(OnDamage());
            }
            if(isDamaged)
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
        // ���͸��� ���� ��ȭ
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        // �����ð�
        yield return new WaitForSeconds(0.5f);
        // ���͸��� ���� ����
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        // �����ð� ��
        isDamaged = false;
        // ������
        yield return new WaitForSeconds(1f);
        yield break;
    }
    void Idle()
    {
        nav.SetDestination(transform.position);
        
        if (Vector3.Distance(transform.position, target.transform.position) <= data.DetectDist)
        {
            curState = State.trace;
        }
        if (curHp <= 0)
        {
            curState = State.die;
        }
    }

    void Trace()
    {
        nav.SetDestination(target.transform.position);
        if(Vector3.Distance(transform.position, target.transform.position) >= data.DetectDist + 5)
        {
            curState = State.idle;
        }
        else if(Vector3.Distance(transform.position,target.transform.position) <= data.AttackRange)
        {
            curState = State.attack;
        }
        if (curHp <= 0)
        {
            curState = State.die;
        }
    }

    void Attack()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > data.AttackRange)
        {
            curState = State.trace;
        }
        else
        {
            StartCoroutine(AttackCoroutine());
        }
        if (curHp <= 0)
        {
            curState = State.die;
        }
    }

    IEnumerator AttackCoroutine()
    {
        yield return null;
    }

    void DIe()
    {
        nav.SetDestination(transform.position);
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.black;
            // 3���� ����
            Destroy(gameObject, 3);
            // �ǰ� �� ���̿ܿ��� �浹���� �ʰ� ����
            gameObject.layer = 9;
        }
    }
}
