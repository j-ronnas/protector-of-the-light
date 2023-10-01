using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Path path;
    int currentIndex;

    Vector2 oldPos;
    Vector2 currentPos;

    float timer = 0f;
    float moveTime = 0.2f;
    [SerializeField]
    Coin coinPrefab;

    [SerializeField]
    GameObject attackAnim;

    EnemySpawner enemySpawner;
    PlayerController player;

    public bool isMarked;
    float timeToMove = 1.5f;
    // Start is called before the first frame update
    public void Init(Path path, EnemySpawner enemySpawner)
    {
        FindAnyObjectByType<TickManager>().AddTickAction(OnTick);
        this.enemySpawner = enemySpawner;
        player = FindAnyObjectByType<PlayerController>();
        this.path = path;
        GetComponent<Health>().AddDeathAction(Die);
        GetComponent<Health>().AddHurtAction(() => { SoundManager.instance.Play("hit"); });

        oldPos = path.GetPosition(0);
        currentPos = path.GetPosition(0);

        currentIndex = -1;
        OnTick();
    }

    // Update is called once per frame
    void Update()
    {

        if (timer <= moveTime)
        {
            transform.position = Vector2.Lerp(oldPos, currentPos, timer / moveTime);
            timer += Time.deltaTime;

        }
    }



    private void OnTick()
    {
        isMarked = false;
        if(timeToMove > 0)
        {
            timeToMove -= 1;
            return;
        }
        timeToMove += 1.5f;

        Debug.Log(Vector2.SqrMagnitude(player.GetPos() - currentPos));
        if (Vector2.SqrMagnitude(player.GetPos()-currentPos) <= 1)
        {
            Instantiate(attackAnim, player.GetPos(), Quaternion.identity);
            player.GetComponent<Health>().Hurt();
            return;
        }

        currentIndex++;
        
        oldPos = currentPos;
        timer = 0;
        currentPos = path.GetPosition(currentIndex);

        if(currentPos == -Vector2.one)
        {
            Debug.Log("Enemy reached end");
            // we reached the end
            isMarked = true;
            FindAnyObjectByType<TickManager>().RemoveTickAction(OnTick);
            FindAnyObjectByType<LightHealth>().Hurt();
            enemySpawner.OnEnemyDeath(this);
            Destroy(gameObject);
            
        }
    }

    public Vector2 GetPos()
    {
        return currentPos;
    }

    public void Die()
    {
        Coin c = Instantiate(coinPrefab, GetPos(), Quaternion.identity, enemySpawner.transform);
        c.Init(GetPos());
        isMarked = true;
        FindAnyObjectByType<TickManager>().RemoveTickAction(OnTick);
        enemySpawner.OnEnemyDeath(this);
        Destroy(gameObject);
    }
}
