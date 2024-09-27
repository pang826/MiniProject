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
    [SerializeField] MeshRenderer mesh;
    [SerializeField] GameObject melee;

    [Header("¼Ó¼º")]
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
        mesh = GetComponent<MeshRenderer>();
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
        Debug.Log(other.gameObject.name);
        if(other == melee)
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
        if(hp <= 0)
        {
            curState = State.die;
        }
    }

    void DIe()
    {
        nav.SetDestination(transform.position);
        MeshRenderer mesh = new MeshRenderer();
        mesh.material.color = Color.black;
    }
    IEnumerator OnDamage()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);

        isDamaged = true;
        yield return delay;
        isDamaged = false;
    }
}
