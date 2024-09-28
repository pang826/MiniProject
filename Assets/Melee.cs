using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    BoxCollider attackRange;
    Coroutine attack;
    public int dmg;
    public float attackSpeed;

    private void Awake()
    {
        dmg = 3;
        attackSpeed = 1f;
        attackRange = GetComponent<BoxCollider>();
        attackRange.enabled = false;
    }
    public void Attack()
    {
        attack = StartCoroutine(Swing());
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.2f);
        attackRange.enabled = true;
        yield return new WaitForSeconds(0.7f);
        attackRange.enabled = false;
        yield return new WaitForSeconds(0.2f);
        yield break;
    }
}
