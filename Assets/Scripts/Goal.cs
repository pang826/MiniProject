using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class Goal : MonoBehaviour
{
    [SerializeField] private int hp;

    [SerializeField] ParticleSystem effect;

    private bool isExplosive;
    public bool IsExplosive;
    private void Start()
    {
        GameManager.Instance.OnDefeatGame += ExplosiveGoal;
    }

    public void TakeDamage()
    {
        hp -= 4;
        if(hp <= 0 && isExplosive == false)
        {
            isExplosive = true;
            GameManager.Instance.OnDefeatGame.Invoke();
        }
    }

    private void ExplosiveGoal()
    {
         effect.Play();
    }
}
