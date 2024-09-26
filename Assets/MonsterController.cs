using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    enum State { idle, patrol, trace, attack, die }
    State curState;

    [Header("¼Ó¼º")]
    [SerializeField] float speed;
}
