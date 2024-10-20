using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    MapManager mapManager;

    [SerializeField]
    PlayerController playerPrefab;
    [SerializeField]
    GameObject goal;

    [SerializeField]
    Path pathPrefab;

    [SerializeField]
    EnemySpawner enemySpawnerPrefab;

    [SerializeField]
    GameObject mainMenu;
    [SerializeField]
    GameObject gameOverMenu;
    [SerializeField]
    GameObject nextlevelMenu;
    [SerializeField]
    GameObject victoryScreen;
    [SerializeField]
    GameObject coverPanel;

    int currentLevel = 0;


    LevelData levelData = new LevelData();

    PlayerController player;

    bool shouldStartLevel = false;

    public void StartLevel(Level level)
    {
        //Create map
        mapManager.CreateLevel(level);

        //Setup health
        LightHealth lightHealth = GetComponent<LightHealth>();
        lightHealth.Restore();


        

        //Place light
        Instantiate(goal, level.goal, Quaternion.identity, transform);

        //Create Enemy Spawner
        EnemySpawner es = Instantiate(enemySpawnerPrefab, transform);
        es.name = "enemy spawner" + currentLevel;

        Path[] paths = new Path[level.enemyPatterns.Length];

        for (int i = 0; i < level.enemyPatterns.Length; i++)
        {
            paths[i] = Instantiate(pathPrefab, mapManager.transform);
            paths[i].Init(level.enemyPatterns[i].startPos, level.goal);
        }

        es.Init(level.enemyPatterns, paths);


        //Init mouse cursor
        GetComponent<BuildingManager>().Init(mapManager, paths);
        print(level.playerSpawn);
        //Create Player
        player = Instantiate(playerPrefab, level.playerSpawn, Quaternion.identity, transform);
        print(player.transform.position);
        player.Init(mapManager, es);
    }

    public void ClearLevel()
    {

        FindAnyObjectByType<TickManager>().ClearTickActions();
        for (int i = mapManager.transform.childCount-1; i >= 0; i--)
        {
            Destroy(mapManager.transform.GetChild(i).gameObject);
        }

        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void OnPlayerDeath()
    {

    }

    public void OnLevelCompleted()
    {
        GetComponent<BuildingManager>().SetMouseCursorMode(MouseMode.MENU);
        player.enabled = false;
        currentLevel++;
        coverPanel.SetActive(true);
        if (currentLevel >= levelData.NumberOfLevels())
        {
            victoryScreen.SetActive(true);
            return;
        }
        nextlevelMenu.SetActive(true);
    }


    public void GameOver()
    {
        GetComponent<BuildingManager>().SetMouseCursorMode(MouseMode.MENU);
        player.enabled = false;

        gameOverMenu.SetActive(true);
        coverPanel.SetActive(true);

    }

    public void StartGame()
    {
        ClearLevel();
        shouldStartLevel = true;
        //Hide menu
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        nextlevelMenu.SetActive(false);
        victoryScreen.SetActive(false);
        coverPanel.SetActive(false);

    }

    public void ToMainMenu()
    {
        currentLevel = 0;
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        nextlevelMenu.SetActive(false);
        victoryScreen.SetActive(false);
        coverPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }



    // Start is called before the first frame update
    void Start()
    {
        //StartLevel(levelData.GetLevel(0));
    }


    // Update is called once per frame
    void Update()
    {
        if (shouldStartLevel)
        {
            StartLevel(levelData.GetLevel(currentLevel));
            shouldStartLevel = false;
        }
    }
}

