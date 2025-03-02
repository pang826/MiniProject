using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Goal goal) && goal.IsExplosive == false)
        {
            goal.TakeDamage();
        }
        if (other.TryGetComponent(out PlayerController player) && player.IsDied == false)
        {
            player.TakeDamage();
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.TryGetComponent(out Goal goal))
    //    {
    //        goal = null;
    //    }
    //    if (other.TryGetComponent(out PlayerController player))
    //    {
    //        player = null;
    //    }
    //}
}
