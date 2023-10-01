using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    List<Enemy> targets;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Sprite loaded;
    [SerializeField]
    Sprite unloaded;

    [SerializeField]
    Transform crossbow;

    EnemySpawner enemySpawner;

    Vector2 position;
    int range = 3;
    int cooldown = 3;
    int timer;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        timer = cooldown;
        targets = new List<Enemy>();
        FindAnyObjectByType<TickManager>().AddTickAction(OnTick);
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTick()
    {
        targets.Clear();
        foreach (Enemy enemy in enemySpawner.GetEnemiesWithin(position, range))
        {
            targets.Add(enemy);
        }
        

        if (timer > 0)
        {

            timer -= 1;
            if(timer == 0)
            {
                crossbow.GetComponentInChildren<SpriteRenderer>().sprite = loaded;
            }
            return;
        }


        for(int i = 0; i < targets.Count; i++)
        {
            if (!targets[i].isMarked)
            {
                crossbow.transform.up = targets[i].transform.position - crossbow.transform.position;

                GameObject go = Instantiate(projectile, transform.position, Quaternion.identity, transform);
                go.GetComponent<Projectile>().Init(targets[i]);
                targets[i].isMarked = true;

                timer = cooldown;
                crossbow.GetComponentInChildren<SpriteRenderer>().sprite = unloaded;

                SoundManager.instance.Play("arrow");
                break;
            }
        }
    }


}
