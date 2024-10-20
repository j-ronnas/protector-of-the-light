using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public abstract class CommonCharacterController : MonoBehaviour
{
    Vector2 oldPos;
    Vector2 currentPos;

    float timer = 0f;
    float moveTime = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void MoveTo(Vector2 position, bool setOld = false){
        
        oldPos = setOld ? position : currentPos;
        currentPos = position;
        timer = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (timer <= moveTime)
        {
            transform.position = Vector2.Lerp(oldPos, currentPos, timer / moveTime);
            timer += Time.deltaTime;

        }
    }
        public Vector2 GetPos()
    {
        return currentPos;
    }

}
