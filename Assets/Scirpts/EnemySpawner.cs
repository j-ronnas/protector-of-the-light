using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyPrefab;

    HashSet<EnemyController> enemies;

    EnemyPattern[] enemyPatterns;
    Path[] enemyPaths;

    int[] timers;
    int[] totalCounts;


    TextMeshProUGUI tmp;
    int totalEnemies;


    public void Init(EnemyPattern[] enemyPatterns, Path[] enemyPaths)
    {
        FindAnyObjectByType<TickManager>().AddTickAction(OnTick);

        this.enemyPatterns = enemyPatterns;
        this.enemyPaths = enemyPaths;
        timers = new int[enemyPatterns.Length];
        totalCounts = new int[enemyPatterns.Length];

        enemies = new HashSet<EnemyController>();

        for (int i = 0; i < enemyPatterns.Length; i++)
        {
            timers[i] = enemyPatterns[i].startDelay;
            totalCounts[i] = enemyPatterns[i].totalCount;
        }

        totalEnemies = GetEnemiesLeft();
        tmp = GameObject.Find("EnemyCount").GetComponent<TextMeshProUGUI>();
        tmp.text = totalEnemies.ToString() + "/" + totalEnemies.ToString();
    }
        

    private void OnTick()
    {
        for (int i = 0; i < enemyPatterns.Length; i++)
        {
            timers[i]--;
            if (timers[i] < 0 && totalCounts[i] > 0)
            {
                EnemyController go = Instantiate(enemyPrefab, enemyPaths[i].GetPosition(0), Quaternion.identity, transform);
                go.Init(enemyPaths[i].GetPosition(0), this);
                enemies.Add(go);
                timers[i] = enemyPatterns[i].spacing;
                totalCounts[i]--;
            }
        }

        int enemiesLeft = GetEnemiesLeft();

        if(enemiesLeft <= 0)
        {
            FindAnyObjectByType<GameManager>().OnLevelCompleted();
        }

        tmp.text = GetEnemiesLeft().ToString() + "/" + totalEnemies.ToString();
    }



    public EnemyController GetEnemyOn(Vector2 pos)
    {
        return enemies.FirstOrDefault(e => e.GetPos() == pos);
    }

    public IEnumerable GetEnemiesWithin(Vector2 pos, int range)
    {
        IEnumerable enemiesInRange = enemies.Where(e => Mathf.Abs(e.GetPos().x - pos.x) <= range && Mathf.Abs(e.GetPos().y - pos.y) <= range);

        return enemiesInRange;
    }


    public void OnEnemyDeath(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }

    private int GetEnemiesLeft()
    {
        int sum = 0;
        foreach (int item in totalCounts)
        {
            sum += item;
        }

        return sum + enemies.Count;
    }



}

