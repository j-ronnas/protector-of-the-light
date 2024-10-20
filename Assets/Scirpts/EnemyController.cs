using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : CommonCharacterController
{
    Path path;
    int currentIndex;
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

        MoveTo(path.GetPosition(0), true);

        currentIndex = -1;
        OnTick();
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

       // Attack player if close by
        if (Vector2.SqrMagnitude(player.GetPos()-GetPos()) <= 1)
        {
            Instantiate(attackAnim, player.GetPos(), Quaternion.identity);
            player.GetComponent<Health>().Hurt();
            return;
        }

        currentIndex++;
        
        Vector2 targetPos = path.GetPosition(currentIndex);

        if(targetPos == -Vector2.one)
        {
            Debug.Log("Enemy reached end");
            // we reached the end
            isMarked = true;
            FindAnyObjectByType<TickManager>().RemoveTickAction(OnTick);
            FindAnyObjectByType<LightHealth>().Hurt();
            enemySpawner.OnEnemyDeath(this);
            Destroy(gameObject);
            
        }
        MoveTo(targetPos);
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
