using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtScreen : MonoBehaviour
{

    float timer;
    float animTime = 0.5f;

    Image image;

    Color color;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        timer = 0;
        color = image.color;
        image.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >0)
        {
            timer -= Time.deltaTime;
        }
        image.color = Color.Lerp(color, Color.clear, 1- timer / animTime);
    }

    public void PlayAnim()
    {
        timer = animTime;
    }

}
