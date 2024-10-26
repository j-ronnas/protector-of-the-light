using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class DungeonRoomTester : MonoBehaviour
{
    DungeonGenerator dungeonGenerator;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject doorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        DisplayMap();

    }

    void DisplayMap(){
        DungeonGenerator dg = new DungeonGenerator(10);
        dg.GenerateDungeon();
        foreach(KeyValuePair<Vector2Int, DungeonRoom> room in dg.GetRoomMap()){
            Instantiate(roomPrefab, (Vector2)room.Key, Quaternion.identity);
            foreach(Vector2Int door in room.Value.doorDirections){
                Vector2 pos = room.Key + ((Vector2)door)*0.45f;
                Instantiate(doorPrefab, pos, Quaternion.identity);
            }
        }

    }

    void RunTests(){
        // Create an instance of the test class
        var dungeonRoomTests = new DungeonRoomTests();
        
        // Set up the test environment (initialize the room list)
        dungeonRoomTests.Setup();

        try
        {
            // Call each test explicitly
            print("Running GetRandomRoom_NoMustHaveOrCannotHaveDoors_ReturnsAnyRoom...");
            dungeonRoomTests.GetRandomRoom_NoMustHaveOrCannotHaveDoors_ReturnsAnyRoom();
            print("Success!");

            print("Running GetRandomRoom_SpecificMustHaveDoor_Up_ReturnsRoomWithUp...");
            dungeonRoomTests.GetRandomRoom_SpecificMustHaveDoor_Up_ReturnsRoomWithUp();
            print("Success!");

            print("Running GetRandomRoom_SpecificCannotHaveDoor_Down_ReturnsRoomWithoutDown...");
            dungeonRoomTests.GetRandomRoom_SpecificCannotHaveDoor_Down_ReturnsRoomWithoutDown();
            print("Success!");

            print("Running GetRandomRoom_MustHaveUpAndLeft_CannotHaveDown_ReturnsCorrectRoom...");
            dungeonRoomTests.GetRandomRoom_MustHaveUpAndLeft_CannotHaveDown_ReturnsCorrectRoom();
            print("Success!");

            print("Running GetRandomRoom_NoRoomMatches_ReturnsNull...");
            dungeonRoomTests.GetRandomRoom_NoRoomMatches_ReturnsNull();
            print("Success!");

            print("Running GetRandomRoom_EmptyMustHave_ReturnsRoomWithoutCannotHaveDoors...");
            dungeonRoomTests.GetRandomRoom_EmptyMustHave_ReturnsRoomWithoutCannotHaveDoors();
            print("Success!");

        }
        catch (Exception ex)
        {
            // Catch any exception and print it
            print($"Test failed: {ex.Message}");
        }
    }

  
}
