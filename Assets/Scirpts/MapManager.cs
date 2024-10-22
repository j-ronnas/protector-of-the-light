using System;

using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;


public class MapManager : MonoBehaviour
{
    [SerializeField]
    GameObject wallPrefab;
    [SerializeField]
    GameObject floorPrefab;

    [SerializeField]
    Sprite[] tileset;

    [SerializeField]
    Sprite floorWallSprite;
    [SerializeField]
    Sprite normalFloorSprite;


    Dictionary<Vector2, Tile> map;

    readonly int mapHeight = 17;
    readonly int mapWidth = 23;



    public void CreateLevel(Level level)
    {
        ReadMap(level.fileName);
        InstantiateMap();
    }

    private void ReadMap(string fileName)
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

    }


    private void InstantiateMap()
    {
        foreach (var item in map)
        {
            if(item.Value.type == "f")
            {
                GameObject floor = Instantiate(floorPrefab, item.Key, Quaternion.identity, transform);
                floor.transform.position += new Vector3(0, 0, 1f);
                floor.GetComponent<SpriteRenderer>().sprite = DetermineFloorSprite(item.Key);
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
        if ( map[vector + Vector2.up].type == "w")
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
        if(vector2.y >= mapHeight || map[vector2 + Vector2.up].type == "w")
        {
            index += 1;
        }
        //West
        if (vector2.x <= 0 || map[vector2 + Vector2.left].type == "w")
        {
            index += 2;
        }
        //East
        if (vector2.x >= mapWidth || map[vector2 + Vector2.right].type == "w")
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



    public  List<Vector2> findPath(Vector2 start, Vector2 end)
    {
        
        Dictionary<Vector2, Vector2> toPrvious = new Dictionary<Vector2, Vector2>();

        Queue<Vector2> uncheckedRegions = new Queue<Vector2>();
        HashSet<Vector2> checkedRegions = new HashSet<Vector2>();
        uncheckedRegions.Enqueue(start);

        while (uncheckedRegions.Count > 0)
        {
            Vector2 currentRegion = uncheckedRegions.Dequeue();
            HashSet<Vector2> neighbors = GetNeighbors(currentRegion);
            foreach (Vector2 item in neighbors)
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

    List<Vector2> createPath(Dictionary<Vector2, Vector2> toPrvious, Vector2 start, Vector2 end)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 current = end;
        while (current != start)
        {
            path.Add(current);
            current = toPrvious[current];

        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    public HashSet<Vector2> GetTilesInRange(Vector2 startPos, int range ){
        HashSet<Vector2> tiles = new HashSet<Vector2>();
        tiles.Add(startPos);
        for(int i = 0; i < range; i ++){
            HashSet<Vector2> newTiles = new HashSet<Vector2>();
            foreach(Vector2 v in tiles){
                newTiles.UnionWith(GetNeighbors(v));
            }

            tiles.UnionWith(newTiles);
            
        }

        return tiles;
    }

    public  HashSet<Vector2> GetNeighbors(Vector2 vector)
    {
        HashSet<Vector2> neighbors = new HashSet<Vector2>();
        if (vector.y < mapHeight && map[vector + Vector2.up].isPassable)
        {
            neighbors.Add(vector + Vector2.up);
        }
        //West
        if (vector.x > 0 && map[vector + Vector2.left].isPassable)
        {
            neighbors.Add(vector + Vector2.left);
        }
        //East
        if (vector.x < mapWidth && map[vector + Vector2.right].isPassable)
        {
            neighbors.Add(vector + Vector2.right);
        }
        //South
        if (vector.y > 0 && map[vector + Vector2.down].isPassable)
        {
            neighbors.Add(vector + Vector2.down);
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
