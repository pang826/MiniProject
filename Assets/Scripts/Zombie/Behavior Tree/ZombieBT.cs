using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBT : Tree
{
    [SerializeField] Transform player;

    [SerializeField] Transform transform;

    [SerializeField] Animator anim;

    [SerializeField] Collider attackRangeCol;

    [SerializeField] ZombieData zData;

    public bool IsDamaged;

    public bool IsAttack;

    [SerializeField] private int hp;
    public int Hp {  get { return hp; } }

    private float speed;

    private float dmg;

    private void Awake()
    {
        hp = zData.Hp;
        speed = zData.Speed;
        dmg = zData.Dmg;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override Node SetUpBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {
            new DieNode(transform, anim, this),
            new DamagedNode(anim, this),
            new SequenceNode(new List<Node>
            {
                new CheckAttackRangeInPlayer(player, transform, anim, this),
                new AttackPlayerNode(player, transform, anim, this)
            }),
            new SequenceNode(new List<Node>
            {
                new CheckPlayerIsNearNode(transform, anim),
                new ChasePlayerNode(player, transform, anim, speed, IsAttack)
            })
        });
        return root;
    }

    public void StartAttackRoutine()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.7f);
        attackRangeCol.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackRangeCol.enabled = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.GetComponent<PlayerController>() && IsDamaged == false && collision.collider is BoxCollider)
        {
            StartCoroutine(DamagedRoutine());
        }
    }

    IEnumerator DamagedRoutine()
    {
        IsDamaged = true;
        hp -= 3;
        yield return new WaitForSeconds(0.2f);
        IsDamaged = false;
        yield break;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            hp -= 3;
        }
    }
}
