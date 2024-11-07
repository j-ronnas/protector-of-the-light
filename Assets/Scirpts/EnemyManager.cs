using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyController enemyPrefab;

    HashSet<EnemyController> enemies;

    List<Vector2Int> enemySpawns;

    public void Init(List<Vector2Int> enemySpawns)
    {
        this.enemySpawns = new List<Vector2Int>();
        enemies = new HashSet<EnemyController>();
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




}

