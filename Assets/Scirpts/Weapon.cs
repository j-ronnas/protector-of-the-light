using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    GameObject attackAnim;
    [SerializeField]
    protected GameObject tileIndicator;


    protected Dictionary<Vector2Int, GameObject> availableTiles;
    Vector2Int prevTile = -Vector2Int.one;

    EnemySpawner enemySpawner;
    TickManager tickManager;
    protected MapManager mapManager;

    public void Init(MapManager mapManager, EnemySpawner enemySpawner){
        this.mapManager = mapManager;
        this.enemySpawner = enemySpawner;

        tickManager = FindObjectOfType<TickManager>();
        tickManager.AddTickAction(ClearIndicators);

        availableTiles = new Dictionary<Vector2Int, GameObject>();

        
    }

    void Update(){
  
        HashSet<Vector2> highlightedTiles = HighlightTiles();
        print(highlightedTiles.Count);
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            print("attacking");
            print("attacking on: " + highlightedTiles.Count);
            Attack(highlightedTiles);
        }
    }

    
    private void Attack(HashSet<Vector2> attackPositions)
    {
        print(attackPositions.Count);
        foreach(Vector2 pos in attackPositions){
            Attack(pos);

        }

        tickManager.TriggerTick();
    }

    private void Attack(Vector2 pos){
        Instantiate(attackAnim, pos, Quaternion.identity);
        
        EnemyController e = enemySpawner.GetEnemyOn(pos);
        if(e != null)
        {
            e.GetComponent<Health>().Hurt();
        }
    }

    public void ClearIndicators(){
        foreach(GameObject go in availableTiles.Values){
            Destroy(go);
        }
        availableTiles.Clear();
        prevTile = -Vector2Int.one;
    }


    public virtual HashSet<Vector2> HighlightTiles(){
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

            return new HashSet<Vector2>{selectedTile};          
        }
        return  new HashSet<Vector2>();
    }


    
    public abstract void SelectWeapon(Vector2 position);
}
