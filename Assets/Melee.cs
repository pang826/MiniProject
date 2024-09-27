using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    BoxCollider attackRange;
    Coroutine attack;
    bool isAttack;
    public int dmg;
    [SerializeField] float attackSpeed;

    private void Awake()
    {
        dmg = 3;
        attackSpeed = 0.3f;
        attackRange = GetComponent<BoxCollider>();
        attackRange.enabled = false;
        isAttack = false;
    }
    void Attack()
    {
        StopCoroutine(attack);
        attack = StartCoroutine(Swing());
    }
    IEnumerator Swing()
    {
        yield return new WaitForSeconds(attackSpeed - 0.1f);
        isAttack = true;
        attackRange.enabled = true;
        yield return new WaitForSeconds(attackSpeed);
        attackRange.enabled = false;
        yield return new WaitForSeconds(attackSpeed);
        isAttack = false;
    }
}
