using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelData
{
    Level[] levels;

    public int NumberOfLevels()
    {
        return levels.Length;
    }
    public Level GetLevel(int index)
    {
        return levels[index];
    }

    public LevelData()
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





    }



}



public class Level
{
    public Vector2 playerSpawn;
    public Vector2 goal;
    public EnemyPattern[] enemyPatterns;

    public string fileName;
}


public class EnemyPattern
{
    public Vector2 startPos;
    public int startDelay;
    public int spacing;
    public int totalCount;
}