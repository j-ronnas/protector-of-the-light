using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 9f;

    EnemyController target;

    public void Init(EnemyController target)
    {
        transform.up = target.GetPos() - (Vector2)transform.position;
        this.target = target;
    }

    private void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.up = target.GetPos() - (Vector2)transform.position;
        transform.position += transform.up * Time.deltaTime* speed;
        if(Vector2.SqrMagnitude((Vector2)transform.position - target.GetPos()) < 0.1f)
        {
            target.GetComponent<Health>().Hurt();
            Destroy(gameObject);
        }
    }

}
