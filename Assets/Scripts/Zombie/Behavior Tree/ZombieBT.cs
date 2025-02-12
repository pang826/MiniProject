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

    private bool isDamaged;

    private int hp;

    private float speed;

    private float dmg;

    private void Awake()
    {
        hp = zData.Hp;
        speed = zData.Speed;
        dmg = zData.Dmg;
    }

    protected override Node SetUpBehaviorTree()
    {
        Node root = new SelectorNode(new List<Node>
        {
            new DieNode(transform, anim, hp),
            new DamagedNode(anim, isDamaged),
            new SequenceNode(new List<Node>
            {
                new CheckAttackRangeInPlayer(player, transform, anim),
                new AttackPlayerNode(player, transform, anim, this)
            }),
            // TODO : 공격 노드 추가
            new SequenceNode(new List<Node>
            {
                new CheckPlayerIsNearNode(transform, anim),
                new ChasePlayerNode(player, transform, anim, speed)
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
        yield return new WaitForSeconds(1.2f);
        attackRangeCol.enabled = true;
        yield return new WaitForSeconds(0.2f);
        attackRangeCol.enabled = false;
        yield break;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.GetComponent<PlayerController>() && isDamaged == false)
        {
            StartCoroutine(DamagedRoutine());
        }
    }

    IEnumerator DamagedRoutine()
    {
        isDamaged = true;
        hp -= 3;
        yield return new WaitForSeconds(0.2f);
        isDamaged = false;
        yield break;
    }
}
