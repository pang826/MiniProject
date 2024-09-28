using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public enum State { idle, patrol, trace, attack, die }
    public State curState;
    [SerializeField] GameObject target;
    [SerializeField] NavMeshAgent nav;
    [SerializeField] MeshRenderer[] meshs;
    [SerializeField] GameObject melee;

    [Header("�Ӽ�")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    [SerializeField] int hp;
    public int dmg;
    [SerializeField] float detectDist;
    [SerializeField] float attackRange;
    bool isDamaged;
    private void Awake()
    {
        speed = 5f;
        hp = 20;
        dmg = 3;
        detectDist = 30;
        attackRange = 3;
        isDamaged = false;
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        curState = State.idle;
    }
    private void Start()
    {
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
                hp -= meleee.dmg;
                StartCoroutine(OnDamage());
            }
        }
    }
    
    void Idle()
    {
        nav.SetDestination(transform.position);
        
        if (Vector3.Distance(transform.position, target.transform.position) <= detectDist)
        {
            curState = State.trace;
        }
    }

    void Trace()
    {
        nav.SetDestination(target.transform.position);
        if(Vector3.Distance(transform.position, target.transform.position) >= detectDist + 5)
        {
            curState = State.idle;
        }
        else if(Vector3.Distance(transform.position,target.transform.position) <= attackRange)
        {
            curState = State.attack;
        }
    }

    void Attack()
    {
        if(Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            curState = State.trace;
        }
        else
        {
            StartCoroutine(AttackCoroutine());
        }
        if(hp <= 0)
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
    IEnumerator OnDamage()
    {
        WaitForSeconds delay = new WaitForSeconds(0.8f);

        // �������� �޾Ҵ����� ���� ����
        isDamaged = true;
        // ���͸��� ���� ��ȭ
        foreach ( MeshRenderer mesh in meshs)
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
}
