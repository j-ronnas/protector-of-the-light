using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DungeonRoomTests
{
    DungeonGenerator dungeonGenerator;

    public void Setup()
    {
        dungeonGenerator = new DungeonGenerator(10);
    }
    public void GetRandomRoom_NoMustHaveOrCannotHaveDoors_ReturnsAnyRoom()
    {
        var mustHaveDoors = new HashSet<Vector2Int>();
        var cannotHaveDoors = new HashSet<Vector2Int>();

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNotNull(result); // Should return any room
    }

    public void GetRandomRoom_SpecificMustHaveDoor_Up_ReturnsRoomWithUp()
    {
        var mustHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, 1) }; // Must have Up door
        var cannotHaveDoors = new HashSet<Vector2Int>();

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.doorDirections.Contains(new Vector2Int(0, 1)));
    }

    public void GetRandomRoom_SpecificCannotHaveDoor_Down_ReturnsRoomWithoutDown()
    {
        var mustHaveDoors = new HashSet<Vector2Int>();
        var cannotHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, -1) }; // Cannot have Down door

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.doorDirections.Contains(new Vector2Int(0, -1)));
    }


    public void GetRandomRoom_MustHaveUpAndLeft_CannotHaveDown_ReturnsCorrectRoom()
    {
        var mustHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, 1), new Vector2Int(-1, 0) }; // Must have Up and Left
        var cannotHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, -1) }; // Cannot have Down

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.doorDirections.Contains(new Vector2Int(0, 1)));
        Assert.IsTrue(result.doorDirections.Contains(new Vector2Int(-1, 0)));
        Assert.IsFalse(result.doorDirections.Contains(new Vector2Int(0, -1)));
    }

    public void GetRandomRoom_NoRoomMatches_ReturnsNull()
    {
        var mustHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, 1) }; // Must have Up
        var cannotHaveDoors = new HashSet<Vector2Int> { new Vector2Int(0, 1) }; // Cannot have Up

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNull(result); // No room can have and not have the same door simultaneously
    }

    public void GetRandomRoom_EmptyMustHave_ReturnsRoomWithoutCannotHaveDoors()
    {
        var mustHaveDoors = new HashSet<Vector2Int>(); // No required doors
        var cannotHaveDoors = new HashSet<Vector2Int> { new Vector2Int(-1, 0) }; // Cannot have Left door

        var result = dungeonGenerator.GetRandomRoom(mustHaveDoors, cannotHaveDoors);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.doorDirections.Contains(new Vector2Int(-1, 0)));
    }
}
