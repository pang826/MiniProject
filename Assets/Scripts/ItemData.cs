using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "ScriptableObject/Item Data", order = int.MaxValue)]
public class ItemData : ScriptableObject
{
    [SerializeField] private string name;
    public string itemName { get { return name; } }

    [SerializeField] private float weight;
    public float Weight { get { return weight; } }

    public enum Type { Weapon, Treat, Material }

    public Type type;
}
