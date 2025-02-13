using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerState
{
    public E_PlayerState CurState; // 상태 확인용
    private IPlayerState state;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ChangeState(E_PlayerState.Idle);
    }

    private void Update()
    {
        state.OnUpdate();
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
                return new PlayerWalkState(this, anim);
            case E_PlayerState.Run:
                return new PlayerRunState(this, anim);
            case E_PlayerState.Damaged:
                return new PlayerDamagedState(this, anim);
            case E_PlayerState.Attack:
                return new PlayerAttackState(this, anim);
            case E_PlayerState.Die:
                return new PlayerDieState(this, anim);
            default:
                return null;

        }
    }
}
