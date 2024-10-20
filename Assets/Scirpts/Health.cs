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

    float yScale;
    private void Start()
    {
        yScale = healthIndicator.transform.localScale.y;
        SetHealth(maxHealth);
        
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
        SetHealth(maxHealth);
        
    }


    public void Hurt(int amount = 1)
    {
        if(OnHurt != null)
        {
            OnHurt();
        }
        


        SetHealth(health - amount);
        
        if (health <= 0)
        {
            OnDeath();

            OnDeath = null;
            OnHurt = null;
        }
    }

    void SetHealth(int health){
        this.health = health;
        healthIndicator.transform.localScale = new Vector3(((float)this.health) / maxHealth, yScale, 1);

    }



}
