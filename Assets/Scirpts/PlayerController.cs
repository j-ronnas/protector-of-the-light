using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject attackAnim;

    MapManager mapManager;
    public EnemySpawner enemySpawner;
    TickManager tickManager;

    Vector2 oldPos;
    Vector2 currentPos;

    float timer = 0f;
    float moveTime = 0.2f;

    Vector2 spawnPos;

    // Start is called before the first frame update
    public void Init(MapManager mapManager, EnemySpawner enemySpawner )
    {
        this.mapManager = mapManager;
        this.enemySpawner = enemySpawner;
        tickManager = FindAnyObjectByType<TickManager>();
        HurtScreen hs = FindAnyObjectByType<HurtScreen>();
        GetComponent<Health>().AddHurtAction(hs.PlayAnim);
        GetComponent<Health>().AddHurtAction(() => { SoundManager.instance.Play("hurt"); });
        GetComponent<Health>().AddDeathAction(OnDeath);


        oldPos = transform.position;
        spawnPos = transform.position;
        currentPos = oldPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveTo(currentPos + Vector2.left);
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveTo(currentPos + Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveTo(currentPos + Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveTo(currentPos + Vector2.right);
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attack();
            if (FindAnyObjectByType<Projectile>() == null)
            {
                tickManager.TriggerTick();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(FindAnyObjectByType<Projectile>() == null)
            {
                tickManager.TriggerTick();
            }
        }
        
        if(timer <= moveTime)
        {
            transform.position = Vector2.Lerp(oldPos, currentPos, timer / moveTime);
            timer += Time.deltaTime;
            
        }
            
    }

    private void MoveTo(Vector2 position)
    {
        if (!mapManager.CanMove(position) || FindAnyObjectByType<Projectile>() != null)
        {
            return;
        }
        else
        {
            oldPos = currentPos;
            currentPos = position;
            timer = 0;
            tickManager.TriggerTick();
        }
    }

    private void Attack()
    {
        Enemy e = null;
        foreach (Vector2 vector in mapManager.GetNeighbors(currentPos))
        {
            e = enemySpawner.GetEnemyOn(vector);
            if(e != null)
            {
                break;
            }

        }
        if(e == null)
        {
            Instantiate(attackAnim, currentPos + Vector2.right, Quaternion.identity);
            
            return;
        }

        Instantiate(attackAnim, e.GetPos(), Quaternion.identity);
        e.GetComponent<Health>().Hurt();

    }

    private void OnDeath()
    {
        FindAnyObjectByType<GameManager>().GameOver();
        
    }

    public Vector2 GetPos()
    {
        return currentPos;
    }
}
