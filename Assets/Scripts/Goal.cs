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

    private void Start()
    {
        GameManager.Instance.OnDefeatGame += ExplosiveGoal;
    }

    public void TakeDamage()
    {
        hp -= 4;
        if(hp <= 0)
            GameManager.Instance.OnDefeatGame.Invoke();
    }

    private void ExplosiveGoal()
    {
        if(isExplosive == false)
        {
            isExplosive = true;
            effect.Play();
        }
    }
}
