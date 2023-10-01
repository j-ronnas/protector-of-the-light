using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAnim : MonoBehaviour
{
    Transform bouncingChild;

    float timer;
    float bounceTime = 0.3f;

    float bounceHeight = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        bouncingChild = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        //bouncingChild.localPosition= new Vector2(0, Mathf.PingPong(Time.time, 0.15f));
        bouncingChild.localPosition = new Vector2(0, bounceHeight* Mathf.Sin(Mathf.PI*2*timer/bounceTime));
        timer += Time.deltaTime;
        if (timer >= bounceTime)
        {
            timer = 0;
        }
    }
}
