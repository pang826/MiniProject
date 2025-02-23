using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerState
{
    public E_PlayerState CurState; // 상태 확인용
    private IPlayerState state;

    private Animator anim;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private int hp = 10;
    public int Hp {  get { return hp; } set { hp = value; } }

    private Rigidbody rigid;

    private bool isDamaged;
    private bool isDied;

    [SerializeField] BoxCollider collider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        ChangeState(E_PlayerState.Idle);
    }

    private void Update()
    {
        if (isDied == false && hp <= 0)
        {
            isDied = true;
            ChangeState(E_PlayerState.Die);
        }

        if (isDied == false)
        {
            state.OnUpdate();
        }

        if(Input.GetMouseButtonDown(1))
        {
            ChangeState(E_PlayerState.Attack);
        }
    }

    private void FixedUpdate()
    {
        state.OnFixedUpdate();
    }

    public void ChangeState(E_PlayerState changeState)
    {
        state?.OnExit();
        state = SelectState(changeState);
        CurState = changeState; // 상태 확인용
        state.OnEnter();
    }

    private IPlayerState SelectState(E_PlayerState changeState)
    {
        switch(changeState) 
        {
            case E_PlayerState.Idle:
                return new PlayerIdleState(this, anim);
            case E_PlayerState.Walk:
                return new PlayerWalkState(this, anim, walkSpeed, rigid);
            case E_PlayerState.Run:
                return new PlayerRunState(this, anim, rigid, runSpeed);
            case E_PlayerState.Damaged:
                return new PlayerDamagedState(this, anim);
            case E_PlayerState.Attack:
                return new PlayerAttackState(this, anim, collider);
            case E_PlayerState.Die:
                return new PlayerDieState(this, anim);
            default:
                return null;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.layer == 7 && isDied == false && isDamaged == false && other.GetComponent<BoxCollider>())
        {
            StartCoroutine(DamageRoutine());
        }
    }

    IEnumerator DamageRoutine()
    {
        isDamaged = true;
        ChangeState(E_PlayerState.Damaged);
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
        yield break;
    }

    
}
