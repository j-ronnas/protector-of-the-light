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
    EnemyManager enemySpawnerPrefab;

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

    public void StartLevel(Level level, Vector2Int doorDirection)
    {
        //Create map
        mapManager.CreateLevel(level);

        //Create Enemy Spawner
        EnemyManager es = Instantiate(enemySpawnerPrefab, transform);
        es.name = "enemy spawner" + currentLevel;


        es.Init(level.enemySpawns);
        //Create Player
        player = Instantiate(playerPrefab, (Vector2)level.GetDoorsInDirection(doorDirection)[0], Quaternion.identity, transform);
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


    // Update is called once per frame
    void Update()
    {
        if (shouldStartLevel)
        {
            StartLevel(levelData.GetLevel(currentLevel), Vector2Int.down);
            shouldStartLevel = false;
        }
    }
}

