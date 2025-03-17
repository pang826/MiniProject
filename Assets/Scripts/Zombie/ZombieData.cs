using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStats", menuName = "GameData/ZombieStats")]
public class ZombieData : ScriptableObject
{
    public int Hp;

    public float Speed;

    public float Dmg;
}
