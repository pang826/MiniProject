using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBT : Tree
{
    [SerializeField] private Transform player;//

    [SerializeField] private Animator anim;//

    [SerializeField] private Collider attackRangeCol;//

    [SerializeField] private ZombieData zData;//

    public bool IsDamaged;

    public bool IsAttack;

    [SerializeField] private int hp;//
    public int Hp {  get { return hp; } }

    private float speed;//

    private float dmg;//

    private void Awake()
    {
        hp = zData.Hp;
        speed = zData.Speed;
        dmg = zData.Dmg;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        base.Start();
        Debug.Log("ZombieBT Start() ½ÇÇàµÊ, enabled »óÅÂ: ");
    }

    protected override Node SetUpBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {

            new DieNode(this.transform, anim, this),
            new DamagedNode(anim, this),
            new SequenceNode(new List<Node>
            {
                new CheckAttackRangeInPlayer(player, this.transform, anim, this),
                new AttackPlayerNode(player, this.transform, anim, this)
            }),
            new SequenceNode(new List<Node>
            {
                new CheckPlayerIsNearNode(this.transform, anim),
                new ChasePlayerNode(player, this.transform, anim, speed, IsAttack)
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
}
