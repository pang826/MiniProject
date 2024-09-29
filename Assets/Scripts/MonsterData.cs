using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [SerializeField]
    private string name;
    public string Name { get { return name; } }

    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }

    [SerializeField]
    private int dmg;
    public int Dmg { get { return dmg; } }

    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField]
    float detectDist;
    public float DetectDist { get { return detectDist; } }

    [SerializeField]
    float attackRange;
    public float AttackRange { get { return attackRange; } }
}
