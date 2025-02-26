using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    private int hp;

    public void TakeDamage()
    {
        hp -= 4;
        if(hp <= 0)
            GameManager.Instance.OnDefeatGame.Invoke();
    }
}
