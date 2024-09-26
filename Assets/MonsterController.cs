using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{

    [SerializeField] enum State { idle, patrol, trace, attack, die }
    [SerializeField] State curState;
    [SerializeField] GameObject target;
    [SerializeField] NavMeshAgent nav;
    Vector3 curPos;
    Ray ray;
    [SerializeField] LayerMask layerMask;
    [Header("¼Ó¼º")]
    [SerializeField] Rigidbody rigid;
    [SerializeField] float speed;
    [SerializeField] int hp;
    [SerializeField] float detectDist;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        speed = 5f;
        hp = 20;
        detectDist = 30f;
        nav = GetComponent<NavMeshAgent>();
        curState = State.idle;
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Move();
        switch (curState)
        {
            case State.idle:
                Idle();
                break;
            case State.patrol:
                break;
            case State.trace:
                Trace();
                break;
            case State.attack:
                break;
            case State.die:
                break;
        }
    }

    private void FixedUpdate()
    {
        
    }
    void Idle()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= detectDist)
        {
            curState = State.trace;
        }
        if(Vector3.Distance(transform.position, target.transform.position) > detectDist)
        {
            nav.SetDestination(transform.position);
        }
    }

    void Patrol()
    {
        
    }

    void Trace()
    {
        nav.SetDestination(target.transform.position);
        if(Vector3.Distance(transform.position, target.transform.position) >= detectDist + 5)
        {
            curState = State.idle;
        }
        //if(Vector3.Distance(transform.position, target.transform.position) < 20)
        //{
        //    nav.SetDestination(target.transform.position);
        //    curPos = transform.position;
        //}
        //
        //if(Vector3.Distance(transform.position, target.transform.position) >= 20)
        //{
        //    transform.position = curPos;
        //    curState = State.idle;
        //}

    }

    void Attack()
    {

    }

    void DIe()
    {

    }
    void Move()
    {
        //rigid.MovePosition(transform.position + (transform.forward * target.transform.position.sqrMagnitude) * speed * Time.deltaTime);
    }

}
