using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum State { walk, run, aim, size }
    State curState;

    Rigidbody rigid;
    Vector3 dir;

    [Header("속성")]
    [SerializeField] float moveSpeed = 5f;   // 기본 움직임 속도
    [SerializeField] float runSpeed = 8f;    // 질주 속도
    [SerializeField] float aimSpeed = 2.5f;    // 조준시 움직임 속도
    float curSpeedType;
    [SerializeField] float turnSpeed = 360f;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputMoveKey();
        Look();

        switch (curState)
        {
            case State.walk:
                curSpeedType = moveSpeed;
                break;
            case State.run:
                curSpeedType = runSpeed;
                break;
            case State.aim:
                curSpeedType = aimSpeed;
                break;
        }
        Debug.Log(curSpeedType);
    }

    private void FixedUpdate()
    {
        Move();
    }
    void InputMoveKey()
    {
        dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            curState = State.run;
        }
        if (Input.GetMouseButton(1))
        {
            curState = State.aim;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetMouseButtonUp(1))
        { curState = State.walk; }
    }

    void Look()
    {
        if (dir != Vector3.zero)
        {
            Quaternion turnDir = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, turnDir, turnSpeed * Time.deltaTime);
        }
    }
    private void Move()
    {
        
        rigid.MovePosition(transform.position + (transform.forward * dir.sqrMagnitude).normalized * curSpeedType * Time.deltaTime);
    }

}
