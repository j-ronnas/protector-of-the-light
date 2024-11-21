using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject floorPrefab;
    [SerializeField]
    List<MapObject> mapObjects;
    Dictionary<string, GameObject> mapObjectPrefabs;

    [SerializeField]
    Sprite[] tileset;

    [SerializeField]
    Sprite floorWallSprite;
    [SerializeField]
    Sprite normalFloorSprite;


    Dictionary<Vector2, Tile> map;


    public static readonly int MAP_HEIGHT = 18;
    public static readonly int MAP_WIDTH = 24;


    public void CreateLevel(Level level)
    {
        this.map = level.map;
        InstantiateMap();
    }

    



    private void InitPrefabMap(){
        mapObjectPrefabs = new Dictionary<string, GameObject>();

        foreach(MapObject mapObject in mapObjects){
            mapObjectPrefabs.Add(mapObject.code, mapObject.prefab);
        }
    }
/* 
    private void ReadCSVMap(string fileName)
    {
        map = new Dictionary<Vector2, Tile>();

        //StreamReader sr = new StreamReader(Resources.Load<TextAsset>(fileName));
        TextAsset ta = Resources.Load<TextAsset>(fileName);


        StringReader reader = new StringReader(ta.text);
        //string line = sr.ReadLine();

        int y = mapHeight;

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            if (y < 0)
            {
                Debug.LogWarning("Too many rows in map file");
                break; 
            }
            string[] arr = line.Split(",");
            for (int x = 0; x < arr.Length; x++)
            {
                map.Add(new Vector2(x, y), new Tile { type = arr[x], isPassable = arr[x] == "f" });
            }
            y--;
        }

    } */


    private void InstantiateMap()
    {
        if (mapObjectPrefabs == null){
            InitPrefabMap();
        }

        foreach (var item in map)
        {
            if(item.Value.type != "w")
            {
                GameObject floor = Instantiate(floorPrefab, item.Key, Quaternion.identity, transform);
                floor.transform.position += new Vector3(0, 0, 1f);
                floor.GetComponent<SpriteRenderer>().sprite = DetermineFloorSprite(item.Key);

                if(mapObjectPrefabs.ContainsKey(item.Value.type)){
                    Instantiate(mapObjectPrefabs[item.Value.type], item.Key, Quaternion.identity, transform);
                }
                

            }
            if(item.Value.type == "w")
            {
                GameObject wall = Instantiate(wallPrefab, item.Key, Quaternion.identity, transform);
                wall.GetComponent<SpriteRenderer>().sprite = DetermineSprite(item.Key);
            }
        }
        
    }

    private Sprite DetermineFloorSprite(Vector2 vector)
    {
        //North
        if ( vector.y < MAP_HEIGHT -1 &&  map[vector + Vector2.up].type == "w")
        {
            map[vector].SetPassable(false);
            return floorWallSprite;
        }
        return normalFloorSprite;
    }


    private Sprite DetermineSprite(Vector2 vector2)
    {
        int index = 0;
        //North
        if(vector2.y >= MAP_HEIGHT -1 || map[vector2 + Vector2.up].type == "w")
        {
            index += 1;
        }
        //West
        if (vector2.x <= 0 || map[vector2 + Vector2.left].type == "w")
        {
            index += 2;
        }
        //East
        if (vector2.x >= MAP_WIDTH -1 || map[vector2 + Vector2.right].type == "w")
        {
            index += 4;
        }
        //South
        if (vector2.y <= 0 || map[vector2 + Vector2.down].type == "w")
        {
            index += 8;
        }


        return tileset[index];
    }

    public bool CanMove(Vector2 position){
        return map.ContainsKey(position) && map[position].isPassable;
    }



    public  List<Vector2Int> findPath(Vector2Int start, Vector2Int end)
    {
        
        Dictionary<Vector2Int, Vector2Int> toPrvious = new Dictionary<Vector2Int, Vector2Int>();

        Queue<Vector2Int> uncheckedRegions = new Queue<Vector2Int>();
        HashSet<Vector2> checkedRegions = new HashSet<Vector2>();
        uncheckedRegions.Enqueue(start);

        while (uncheckedRegions.Count > 0)
        {
            Vector2Int currentRegion = uncheckedRegions.Dequeue();
            HashSet<Vector2Int> neighbors = GetNeighbors(currentRegion);
            foreach (Vector2Int item in neighbors)
            {
                if (checkedRegions.Contains(item))
                {
                    continue;
                }
                toPrvious.Add(item, currentRegion);
                if (item == end)
                {
                    return createPath(toPrvious, start, end);
                }
                uncheckedRegions.Enqueue(item);
                checkedRegions.Add(item);
            }


        }



        return createPath(toPrvious, start, end);
        
    }

    List<Vector2Int> createPath(Dictionary<Vector2Int, Vector2Int> toPrvious, Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = end;
        while (current != start)
        {
            path.Add(current);
            current = toPrvious[current];

        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    public HashSet<Vector2Int> GetTilesInRange(Vector2Int startPos, int range ){
        HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();
        tiles.Add(startPos);
        for(int i = 0; i < range; i ++){
            HashSet<Vector2Int> newTiles = new HashSet<Vector2Int>();
            foreach(Vector2Int v in tiles){
                newTiles.UnionWith(GetNeighbors(v));
            }

            tiles.UnionWith(newTiles);
            
        }

        return tiles;
    }

    public  HashSet<Vector2Int> GetNeighbors(Vector2Int vector)
    {
        HashSet<Vector2Int> neighbors = new HashSet<Vector2Int>();
        if (vector.y < MAP_HEIGHT && map[vector + Vector2.up].isPassable)
        {
            neighbors.Add(vector + Vector2Int.up);
        }
        //West
        if (vector.x > 0 && map[vector + Vector2.left].isPassable)
        {
            neighbors.Add(vector + Vector2Int.left);
        }
        //East
        if (vector.x < MAP_WIDTH && map[vector + Vector2.right].isPassable)
        {
            neighbors.Add(vector + Vector2Int.right);
        }
        //South
        if (vector.y > 0 && map[vector + Vector2.down].isPassable)
        {
            neighbors.Add(vector + Vector2Int.down);
        }

        return neighbors;
    }

    public List<Vector2> GetTilesInLine(Vector2 start, Vector2 end, float stepLength = 0.1f){
        int maxIterations = 1000;
        List<Vector2> tiles = new List<Vector2>();
        tiles.Add(Vector2Int.FloorToInt(start));
        Vector2 direction = end - start;

        Vector2 currentTile = start;
        while (maxIterations > 0){
            currentTile += direction.normalized * stepLength;
            Vector2Int newTile = Vector2Int.FloorToInt(currentTile + direction);
            if(newTile != Vector2Int.FloorToInt(tiles[tiles.Count -1])){
                tiles.Add(newTile);
            }

            if(newTile == Vector2Int.FloorToInt(end)){
                break;
            }

            maxIterations --;
        }
        
        return tiles;
    }

    private Vector2 RoundVector(Vector2 v){
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y) );
    }

    public List<Vector2> GetTilesFromRay(Vector2 start, Vector2 direction, int maxLength, float stepLength = 0.1f){
        int maxIterations = 1000;
        List<Vector2> tiles = new List<Vector2>();
        tiles.Add(RoundVector(start));
        Vector2 currentTile = start;
        while (maxIterations > 0){
            currentTile += direction.normalized * stepLength;
            Vector2 newTile = RoundVector(currentTile + direction);
            
            if(CanMove(newTile) == false){
                break;
            }
            
            if(newTile != RoundVector(tiles[tiles.Count -1])){
                tiles.Add(newTile);
            }

            if(tiles.Count >= maxLength){
                break;
            }

            maxIterations --;
        }
        
        return tiles;
    }



    
}

public class Tile
{
    public string type;
    public bool isPassable;

    public void SetPassable(bool value)
    {
        this.isPassable = value;
    }
}

[Serializable]
public struct MapObject{
    public string code;
    public GameObject prefab;
}
