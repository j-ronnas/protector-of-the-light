using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class DungeonGenerator
{
    Vector2Int[] allDirections = {Vector2Int.down, Vector2Int.left, Vector2Int.up, Vector2Int.right};

    //Properties
    int numberOfRooms;

    List<DungeonRoom> templateRooms;
    Dictionary<Vector2Int, DungeonRoom> roomMap;

    public DungeonGenerator(int numberOfRooms){
        this.numberOfRooms = numberOfRooms;

        //TODO: init with data
        templateRooms = new List<DungeonRoom>
        {
            // Room with only an Up door
            new DungeonRoom
            {
                templateFileName = "room_N",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1) }
            },
            // Room with only a Down door
            new DungeonRoom
            {
                templateFileName = "room_S",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1) }
            },
            // Room with only a Left door
            new DungeonRoom
            {
                templateFileName = "room_W",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(-1, 0) }
            },
            // Room with only a Right door
            new DungeonRoom
            {
                templateFileName = "room_E",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(1, 0) }
            },
            // Room with Up and Down doors
            new DungeonRoom
            {
                templateFileName = "room_NS",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1) }
            },
            // Room with Up and Left doors
            new DungeonRoom
            {
                templateFileName = "room_NW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(-1, 0) }
            },
            // Room with Up and Right doors
            new DungeonRoom
            {
                templateFileName = "room_NE",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 0) }
            },
            // Room with Down and Left doors
            new DungeonRoom
            {
                templateFileName = "room_SW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(-1, 0) }
            },
            // Room with Down and Right doors
            new DungeonRoom
            {
                templateFileName = "room_ES",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(1, 0) }
            },
            // Room with Left and Right doors
            new DungeonRoom
            {
                templateFileName = "room_EW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with Up, Down, and Left doors
            new DungeonRoom
            {
                templateFileName = "room_NSW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0) }
            },
            // Room with Up, Down, and Right doors
            new DungeonRoom
            {
                templateFileName = "room_NES",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0) }
            },
            // Room with Up, Left, and Right doors
            new DungeonRoom
            {
                templateFileName = "room_NEW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with Down, Left, and Right doors
            new DungeonRoom
            {
                templateFileName = "room_ESW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with all four doors
            new DungeonRoom
            {
                templateFileName = "room_NESW",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            }
        };
    }

    public Dictionary<Vector2Int, DungeonRoom> GetRoomMap(){
        return roomMap;
    }

    public void GenerateDungeon(){
        GenerateRooms();
    }

    public HashSet<Vector2Int> GenerateMap(){
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        Vector2Int currentRoomPos = Vector2Int.zero;
        roomPositions.Add(currentRoomPos);
        Vector2Int direction = Vector2Int.up;

        for(int i = 0; i < numberOfRooms; i++){
            currentRoomPos += direction;
            roomPositions.Add(currentRoomPos);
            int randomDirectionIndex = Random.Range(0,allDirections.Length);
            if(direction == allDirections[randomDirectionIndex]){
                randomDirectionIndex = (randomDirectionIndex + 1 )%allDirections.Length;
            }
            direction = allDirections[randomDirectionIndex];
            Debug.Log(currentRoomPos);
        }

        return roomPositions;
    }
    public void GenerateRooms(HashSet<Vector2Int> roomPositions){
        roomMap = new Dictionary<Vector2Int, DungeonRoom>();
        foreach(Vector2Int room in roomPositions){
            HashSet<Vector2Int> musts = new HashSet<Vector2Int>();
            HashSet<Vector2Int> cannots = new HashSet<Vector2Int>();
            
            foreach(Vector2Int v in allDirections){
                Vector2Int pos = room + v;
                if(roomPositions.Contains(pos)){
                    musts.Add(v);
                }else{
                    cannots.Add(v);
                }
            }

            DungeonRoom dr = GetRandomRoom(musts, cannots);
            if(dr!= null){
                roomMap.Add(room, dr);
            }else{
                Debug.Log("no available template room here");   
            }
           
        }
    }

    public void GenerateRooms(){
        roomMap = new Dictionary<Vector2Int, DungeonRoom>();
        
        Queue<Vector2Int> openPositions = new Queue<Vector2Int>();
        
        Vector2Int currentRoomPos = Vector2Int.zero;
        Vector2Int direction = Vector2Int.up;
        
        openPositions.Enqueue(currentRoomPos + direction);
        roomMap.Add(currentRoomPos, templateRooms[0]);

        int roomCounter = 0;

        while(openPositions.Count > 0){
            currentRoomPos = openPositions.Dequeue();
            
            HashSet<Vector2Int> musts = new HashSet<Vector2Int>();
            HashSet<Vector2Int> cannots = new HashSet<Vector2Int>();
            foreach(Vector2Int v in allDirections){
                Vector2Int pos = currentRoomPos + v;
                if(roomMap.ContainsKey(pos)){
                    if(roomMap[pos].doorDirections.Contains(-v)){
                        musts.Add(v);
                    }else{
                        cannots.Add(v);
                    }
                }else if(roomCounter > numberOfRooms){
                    cannots.Add(v);
                }
            }

            DungeonRoom dr = GetRandomRoom(musts, cannots);
            if(dr != null){
                roomMap.TryAdd(currentRoomPos, dr); // Why are we happening on the same place twice?
                foreach(Vector2Int door in dr.doorDirections){
                    Vector2Int pos = door + currentRoomPos;
                    if(roomMap.ContainsKey(pos) == false){
                        openPositions.Enqueue(pos);
                    }
                }
            }

            roomCounter ++;
            if(roomCounter > numberOfRooms * 10){
                Debug.Log("Too many rooms tried!");
                break;
            }
        }
    }

    public DungeonRoom GetRandomRoom(HashSet<Vector2Int> mustHaveDoors, HashSet<Vector2Int> cannotHaveDoors){
        
        List<DungeonRoom> eligibleRooms = new List<DungeonRoom>();
        
        foreach(DungeonRoom dr in templateRooms){
            if(mustHaveDoors.IsSubsetOf(dr.doorDirections) && dr.doorDirections.Overlaps(cannotHaveDoors) == false ){
                eligibleRooms.Add(dr);
            }
        }

        if (eligibleRooms.Count == 0){
            return null;

        }

        return eligibleRooms[Random.Range(0, eligibleRooms.Count)];


    }

   
}

public class DungeonRoom{
    public string templateFileName;
    public HashSet<Vector2Int> doorDirections;
    public Vector2[] enemySpawns;
}

