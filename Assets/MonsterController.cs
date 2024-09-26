using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] enum State { idle, patrol, trace, attack, die }
    [SerializeField] State curState;
    [SerializeField] GameObject target;
    [SerializeField] NavMeshAgent nav;
    

    [Header("¼Ó¼º")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    public int hp;
    public int dmg;
    [SerializeField] float detectDist;
    [SerializeField] float attackRange;
    private void Awake()
    {
        speed = 5f;
        hp = 20;
        dmg = 3;
        detectDist = 30;
        attackRange = 3;
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        curState = State.idle;
        
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
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

    }

    void DIe()
    {

    }
}
