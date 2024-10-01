using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloth : MonoBehaviour
{
    [SerializeField] ItemData data;
    [SerializeField] ItemData.Type type;
    void Start()
    {
        type = data.type;
    }
}
