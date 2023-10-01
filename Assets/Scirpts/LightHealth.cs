using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightHealth : MonoBehaviour
{
    int health;
    TextMeshProUGUI tmp;
    HurtScreen hurtScreen;
    GameManager gameManager;
    private void Start()
    {
        tmp = GameObject.Find("Lightlife").GetComponent<TextMeshProUGUI>();
        hurtScreen = FindAnyObjectByType<HurtScreen>();
        gameManager = GetComponent<GameManager>();

        
    }



    public void Restore()
    {
        health = 3;
        tmp.text = health.ToString();
    }

    public void Hurt()
    {
        health--;
        tmp.text = health.ToString();
        hurtScreen.PlayAnim();

        if(health < 0)
        {
            gameManager.GameOver();
        }
    }

}
