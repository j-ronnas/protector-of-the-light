using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    Action OnDeath;
    Action OnHurt;
    int health;

    [SerializeField]
    int maxHealth;

    [SerializeField]
    GameObject healthIndicator;

    GameObject[] indicators;


    private void Start()
    {
        health = maxHealth;
        indicators = new GameObject[maxHealth];
        for (int i = 0; i < maxHealth; i++)
        {
            indicators[i] = Instantiate(healthIndicator, transform);
            indicators[i].transform.localPosition = new Vector2(((float)i / maxHealth) - (float)(maxHealth-1)/(2*maxHealth), -0.7f);
        }
    }

    public void AddDeathAction(Action deathAction)
    {
        OnDeath += deathAction;
    }

    public void AddHurtAction(Action hurtAction)
    {
        OnHurt += hurtAction;
    }

    public void Restore()
    {
        health = maxHealth;
        for (int i = 0; i < maxHealth; i++)
        {
            if (i < health)
            {
                indicators[i].SetActive(true);
            }
            else
            {
                indicators[i].SetActive(false);
            }
        }
    }


    public void Hurt(int amount = 1)
    {
        if(OnHurt != null)
        {
            OnHurt();
        }
        


        health -= amount;
        
        if (health <= 0)
        {
            OnDeath();

            OnDeath = null;
            OnHurt = null;
        }

        for (int i = 0; i < maxHealth; i++)
        {
            if(i < health)
            {
                indicators[i].SetActive(true);
            }
            else
            {
                indicators[i].SetActive(false);
            }
        }
    }



}
