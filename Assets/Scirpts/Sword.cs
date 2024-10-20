using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    [SerializeField]
    GameObject attackAnim;


    [SerializeField]
    GameObject tileIndicator;

    int range = 2;
    MapManager mapManager;
    PlayerController player;
    EnemySpawner enemySpawner;

    Dictionary<Vector2Int, GameObject> availableTiles;

    Vector2Int prevTile = -Vector2Int.one;
    TickManager tickManager;

    public void Init(PlayerController player, MapManager mapManager, EnemySpawner enemySpawner){
        this.player = player;
        this.mapManager = mapManager;
        this.enemySpawner = enemySpawner;

        tickManager = FindObjectOfType<TickManager>();
        tickManager.AddTickAction(ClearIndicators);

        availableTiles = new Dictionary<Vector2Int, GameObject>();

        
    }

    void Update(){
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int selectedTile = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) );

        if(availableTiles.ContainsKey(selectedTile)){
            if(prevTile != selectedTile){
                if(prevTile != -Vector2Int.one){
                    availableTiles[prevTile].GetComponent<SpriteRenderer>().color = Color.white;
                }
                
                availableTiles[selectedTile].GetComponent<SpriteRenderer>().color = Color.green;
                prevTile = selectedTile;
            }


            if(Input.GetMouseButtonDown(0))
            {
                Attack(selectedTile);
            }

            
        }

        
    }

    public void SelectWeapon(Vector2 position){
        availableTiles.Clear();
        foreach(Vector2 v in mapManager.GetTilesInRange(position, range)){
            availableTiles.Add(new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y) ), Instantiate(tileIndicator, v, Quaternion.identity));
        }
    }

    public void ClearIndicators(){
        foreach(GameObject go in availableTiles.Values){
            Destroy(go);
        }
        availableTiles.Clear();
        prevTile = -Vector2Int.one;
    }

    private void Attack(Vector2 pos)
    {
        Instantiate(attackAnim, pos, Quaternion.identity);
        
        EnemyController e = enemySpawner.GetEnemyOn(pos);
        if(e != null)
        {
            e.GetComponent<Health>().Hurt();
        }

        tickManager.TriggerTick();
    }

}
