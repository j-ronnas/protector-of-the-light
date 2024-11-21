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

    Vector2Int currentLevel = Vector2Int.zero;


    LevelData levelData;

    PlayerController player;

    bool shouldStartLevel = false;

    private void Start(){
        print("starting game manager");
        levelData = new LevelData();
    }

    void StartLevel(Level level, Vector2Int doorDirection, bool isEntrance = false)
    {

        FindAnyObjectByType<TickManager>().AddTickAction(OnTick);
        //Create map
        mapManager.CreateLevel(level);

        //Create Enemy Spawner
        EnemyManager es = Instantiate(enemySpawnerPrefab, transform);
        es.name = "enemy spawner" + currentLevel;


        es.Init(level.enemySpawns);
        //Create Player
        if(isEntrance){
            player = Instantiate(playerPrefab, new Vector2(6,12), Quaternion.identity, transform);
        }else{
            player = Instantiate(playerPrefab, (Vector2)level.GetDoorsInDirection(doorDirection)[0], Quaternion.identity, transform);
        }
        
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

 /*        GetComponent<BuildingManager>().SetMouseCursorMode(MouseMode.MENU);
        player.enabled = false;
        currentLevel++;
        coverPanel.SetActive(true);
        if (currentLevel >= levelData.NumberOfLevels())
        {
            victoryScreen.SetActive(true);
            return;
        }
        nextlevelMenu.SetActive(true); */
    }


    void OnTick(){
        Level level = levelData.GetLevel(currentLevel);
        if (level.doors.Contains(player.GetPos())){
            ClearLevel();
            Vector2Int doorDir = level.GetDirectionOfDoor(player.GetPos());
            currentLevel += doorDir;
            StartLevel(levelData.GetLevel(currentLevel), -doorDir);
            
        }
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
        currentLevel = Vector2Int.zero;
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
            print("starting leve");
            print(levelData == null);
            StartLevel(levelData.GetLevel(currentLevel), Vector2Int.up, true);
            shouldStartLevel = false;
        }
    }
}

