using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZombieBT : Tree
{
    [SerializeField] private Transform player;//
    [SerializeField] private Transform goal;
    [SerializeField] private Animator anim;//
    [SerializeField] private Collider attackRangeCol;//
    [SerializeField] private ZombieData zData;//

    public bool IsDamaged;
    public bool IsAttack;
    public bool IsStuck;

    [SerializeField] private int hp;
    public int Hp {  get { return hp; } }
    private float speed;
    private float dmg;

    private void Awake()
    {
        hp = zData.Hp;
        speed = zData.Speed;
        dmg = zData.Dmg;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
    }

    private void Start()
    {
        base.Start();
        Debug.Log("ZombieBT Start() ½ÇÇàµÊ, enabled »óÅÂ: ");
        GameManager.Instance.OnDefeatGame += ResetTarget;
    }

    protected override Node SetUpBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {

            new DieNode(this.transform, anim, this),
            new DamagedNode(anim, this),
            new SequenceNode(new List<Node>
            {
                new CheckGoalNode(this.transform, anim, this, goal),
                new AttackGoalNode(this.transform, anim, this, goal)
            }),
            new SequenceNode(new List<Node>
            {
                new CheckAttackRangeInPlayer(player, this.transform, anim, this),
                new AttackPlayerNode(player, this.transform, anim, this)
            }),
            new SequenceNode(new List<Node>
            {
                new CheckPlayerIsNearNode(this.transform, anim),
                new ChasePlayerNode(player, this.transform, anim, speed, IsAttack, this)
            }),
            new GoToCenterNode(this.transform,  goal, anim, this, speed)
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

    IEnumerator DamagedRoutine()
    {
        IsDamaged = true;
        hp -= 3;
        yield return new WaitForSeconds(0.2f);
        IsDamaged = false;
        yield break;
    }
    IEnumerator StuckRoutine()
    {
        IsStuck = true;
        yield return new WaitForSeconds(1.5f);
        IsStuck = false;
        yield break;
    }
    public void Damaged()
    {
        StartCoroutine(StuckRoutine());
        if (IsDamaged == false)
            StartCoroutine(DamagedRoutine());
    }

    private void ResetTarget()
    {
        goal = null;
    }
}
