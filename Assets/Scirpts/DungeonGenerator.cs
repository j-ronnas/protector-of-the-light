using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DungeonGenerator
{
    //Properties
    int numberOfRooms;

    List<DungeonRoom> templateRooms;
    Dictionary<Vector2Int, DungeonRoom> roomMap;

    public DungeonGenerator(int numberOfRooms){
        this.numberOfRooms = numberOfRooms;

        //TODO: init with data
        templateRooms = new List<DungeonRoom>
        {
            // Room with no doors
            new DungeonRoom
            {
                templateFileName = "Room0",
                doorDirections = new HashSet<Vector2Int> { }
            },
            // Room with only an Up door
            new DungeonRoom
            {
                templateFileName = "Room1",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1) }
            },
            // Room with only a Down door
            new DungeonRoom
            {
                templateFileName = "Room2",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1) }
            },
            // Room with only a Left door
            new DungeonRoom
            {
                templateFileName = "Room3",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(-1, 0) }
            },
            // Room with only a Right door
            new DungeonRoom
            {
                templateFileName = "Room4",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(1, 0) }
            },
            // Room with Up and Down doors
            new DungeonRoom
            {
                templateFileName = "Room5",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1) }
            },
            // Room with Up and Left doors
            new DungeonRoom
            {
                templateFileName = "Room6",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(-1, 0) }
            },
            // Room with Up and Right doors
            new DungeonRoom
            {
                templateFileName = "Room7",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(1, 0) }
            },
            // Room with Down and Left doors
            new DungeonRoom
            {
                templateFileName = "Room8",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(-1, 0) }
            },
            // Room with Down and Right doors
            new DungeonRoom
            {
                templateFileName = "Room9",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(1, 0) }
            },
            // Room with Left and Right doors
            new DungeonRoom
            {
                templateFileName = "Room10",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with Up, Down, and Left doors
            new DungeonRoom
            {
                templateFileName = "Room11",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0) }
            },
            // Room with Up, Down, and Right doors
            new DungeonRoom
            {
                templateFileName = "Room12",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(1, 0) }
            },
            // Room with Up, Left, and Right doors
            new DungeonRoom
            {
                templateFileName = "Room13",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with Down, Left, and Right doors
            new DungeonRoom
            {
                templateFileName = "Room14",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            },
            // Room with all four doors
            new DungeonRoom
            {
                templateFileName = "Room15",
                doorDirections = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0) }
            }
        };
    }


    public void GenerateRooms(){

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

