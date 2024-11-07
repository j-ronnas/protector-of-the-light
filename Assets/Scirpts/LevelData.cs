using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelData
{
    Level[] levels;
    Dictionary<Vector2Int, Level> map;

    public int NumberOfLevels()
    {
        return levels.Length;
    }
    public Level GetLevel(int index)
    {
        return levels[index];
    }

    public LevelData(){
        DungeonGenerator dungeonGenerator =new DungeonGenerator(15);
        dungeonGenerator.GenerateDungeon();

        map = new Dictionary<Vector2Int, Level>();
        foreach(KeyValuePair<Vector2Int, DungeonRoom> dungeonRoom in dungeonGenerator.GetRoomMap()) {
            map.Add(dungeonRoom.Key, new Level(
                ReadPixelMap(dungeonRoom.Value.templateFileName, MapManager.MAP_HEIGHT, MapManager.MAP_WIDTH)
            ));
        }

    }

    private Dictionary<Vector2, Tile> ReadPixelMap(string fileName, int mapHeight, int mapWidth){
        Dictionary<Vector2, Tile> map = new Dictionary<Vector2, Tile>();

        Texture2D texture = Resources.Load<Texture2D>(fileName);
        Color[] colors = texture.GetPixels();

        for (int y = 0;  y < mapHeight; y ++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                string tileType = ColorToTileType(colors[y*mapWidth + x]);
                map.Add(new Vector2(x, y), new Tile { type = tileType, isPassable = tileType != "w" });
            
            }
        }

        return map;
    }

    private string ColorToTileType(Color color){
        if(color == Color.black){
            return "w";
        }
        if(color == Color.white){
            return "f";
        }
        if(color == Color.red){
            return "e";
        }
        if(color == Color.blue){
            return "c";
        }
        if(color == Color.green){
            return "d";
        }

        Debug.LogWarning("Not a recognised color: " + color.ToString());
        return "f";
    }
    /* public LevelData()
    {
        levels = new Level[3];

        levels[0] = new Level
        {
            playerSpawn = new Vector2(9, 9),
            goal = new Vector2(13, 10),
            enemyPatterns = new EnemyPattern[] {
                new EnemyPattern
                {
                    startPos = new Vector2(16, 0),
                    startDelay = 3,
                    spacing = 8,
                    totalCount = 5
                }

            },
            fileName = "lvl1"
        };

        levels[1] = new Level
        {
            playerSpawn = new Vector2(4, 9),
            goal = new Vector2(20, 13),
            enemyPatterns = new EnemyPattern[] { 
                new EnemyPattern
                {
                    startPos = new Vector2(16, 0),
                    startDelay = 3,
                    spacing = 7,
                    totalCount = 5
                },
                new EnemyPattern
                {
                    startPos = new Vector2(4, 0),
                    startDelay = 25,
                    spacing = 7,
                    totalCount = 5
                }

            },
            fileName = "lvl2"
        };

        levels[2] = new Level
        {
            playerSpawn = new Vector2(3, 9),
            goal = new Vector2(13, 14),
            enemyPatterns = new EnemyPattern[] {
                new EnemyPattern
                {
                    startPos = new Vector2(6, 0),
                    startDelay = 3,
                    spacing = 7,
                    totalCount = 6
                },
                new EnemyPattern
                {
                    startPos = new Vector2(14, 0),
                    startDelay = 30,
                    spacing = 5,
                    totalCount = 7
                },
                new EnemyPattern
                {
                    startPos = new Vector2(17, 0),
                    startDelay = 20,
                    spacing = 7,
                    totalCount = 7
                }

            },
            fileName = "lvl3"
        };





    } */



}



public class Level
{
    public List<Vector2Int> doors;
    public List<Vector2Int> enemySpawns;
    public Dictionary<Vector2, Tile> map;
    public Level(Dictionary<Vector2, Tile> map){
        this.map = map;
        doors = new List<Vector2Int>();
        enemySpawns = new List<Vector2Int>();
        foreach(KeyValuePair<Vector2, Tile> tile in map){
            if(tile.Value.type == "d"){
                doors.Add(Vector2Int.FloorToInt(tile.Key));
            }
            if(tile.Value.type == "e"){
                enemySpawns.Add(Vector2Int.FloorToInt(tile.Key));
            }
        }
    }

    public List<Vector2Int> GetDoorsInDirection(Vector2Int direction){
        List<Vector2Int> doorList = new List<Vector2Int>();

        if(direction == Vector2Int.up){
            foreach(Vector2Int door in doors){
                if(door.y == MapManager.MAP_HEIGHT -1){
                    doorList.Add(door);
                }
            }
        }
        if(direction == Vector2Int.down){
            foreach(Vector2Int door in doors){
                if(door.y == 0){
                    doorList.Add(door);
                }
            }
        }
        if(direction == Vector2Int.right){
            foreach(Vector2Int door in doors){
                if(door.x == MapManager.MAP_WIDTH -1){
                    doorList.Add(door);
                }
            }
        }
        if(direction == Vector2Int.left){
            foreach(Vector2Int door in doors){
                if(door.x == 0){
                    doorList.Add(door);
                }
            }
        }

        return doorList;

    }


}


public class EnemyPattern
{
    public Vector2 startPos;
    public int startDelay;
    public int spacing;
    public int totalCount;
}