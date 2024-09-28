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

    [Header("속성")]
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
            if(!isDamaged && curState != State.die)
            {
                hp -= meleee.dmg;
                StartCoroutine(OnDamage());
            }
        }
    }
    IEnumerator OnDamage()
    {
        // 무적시간 시작(데미지를 받았는지에 대한 여부)
        isDamaged = true;
        // 경직
        nav.SetDestination(transform.position);
        // 매터리얼 색상 변화
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }

        // 무적시간
        yield return new WaitForSeconds(0.5f);

        // 무적시간 끝
        isDamaged = false;
        // 재추적
        yield return new WaitForSeconds(0.5f);
        nav.SetDestination(target.transform.position);
        // 매터리얼 색상 복구
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }
    void Idle()
    {
        nav.SetDestination(transform.position);
        
        if (Vector3.Distance(transform.position, target.transform.position) <= detectDist)
        {
            curState = State.trace;
        }
        if (hp <= 0)
        {
            curState = State.die;
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
        if (hp <= 0)
        {
            curState = State.die;
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
        if (hp <= 0)
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
            // 3초후 삭제
            Destroy(gameObject, 3);
            // 피격 시 땅이외에는 충돌하지 않게 설정
            gameObject.layer = 9;
        }
        
    }
    
}
