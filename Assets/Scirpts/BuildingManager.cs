using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField]
    GameObject mouseCursor;

    [SerializeField]
    Sprite mouseCursorSprite;
    [SerializeField]
    Sprite towerSprite;

    [SerializeField]
    GameObject towerPrefab;

    MapManager mapManager;
    Path[] paths;
    

    SpriteRenderer mouseSpriteRenderer;
    MouseMode currentMouseMode;



    int gold;
    int towerPrice = 30;

    TextMeshProUGUI tmp;

    // Start is called before the first frame update

    private void Start()
    {
        SetMouseCursorMode(MouseMode.MENU);
    }
    public void Init(MapManager mapManager, Path[] paths)
    {
        this.paths = paths;
        this.mapManager = mapManager;

        mouseSpriteRenderer = mouseCursor.GetComponent<SpriteRenderer>();
        SetMouseCursorMode(MouseMode.DEFAULT);
        tmp = GameObject.Find("Money").GetComponent<TextMeshProUGUI>();

        SetGold(30);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMouseMode == MouseMode.MENU)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetMouseCursorMode(MouseMode.BUILD_TOWER);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetMouseCursorMode(MouseMode.DEFAULT);
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        Vector2 pos = Camera.main.ScreenToWorldPoint    (Input.mousePosition);
        mouseCursor.transform.position = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y) );


        if(currentMouseMode == MouseMode.BUILD_TOWER)
        {
            mouseSpriteRenderer.color = CanBuild(mouseCursor.transform.position) ? Color.white : Color.red;
        }
        else
        {
            mouseSpriteRenderer.color = Color.white;

        }
    }

    private void HandleClick()
    {
        switch (currentMouseMode)
        {
            case MouseMode.MENU:
            case MouseMode.DEFAULT:
                break;
            case MouseMode.BUILD_TOWER:
                if (!CanBuild(mouseCursor.transform.position))
                {
                    break;
                }
                SetGold(gold - towerPrice);
                Instantiate(towerPrefab, mouseCursor.transform.position, Quaternion.identity, transform);
                SetMouseCursorMode(MouseMode.DEFAULT);
                break;
        }
    }

    public void SetMouseCursorMode(MouseMode mode)
    {
        currentMouseMode = mode;

        switch (currentMouseMode)
        {
            case MouseMode.DEFAULT:
                mouseCursor.SetActive(true);
                mouseCursor.GetComponent<SpriteRenderer>().sprite = mouseCursorSprite;
                break;
            case MouseMode.BUILD_TOWER:
                mouseCursor.SetActive(true);
                mouseCursor.GetComponent<SpriteRenderer>().sprite = towerSprite;
                break;
            case MouseMode.MENU:
                mouseCursor.SetActive(false);
                break;

        }
    }

    private bool CanBuild(Vector2 pos)
    {

        bool isValidSpot = true;
        foreach(Path p in paths)
        {
            isValidSpot &= !p.ContainsPos(pos);
        }

        return isValidSpot && mapManager.CanMove(pos) &&  gold >= towerPrice;
    }

    private void SetGold(int newAmount)
    {
        gold = newAmount;
        tmp.text = gold.ToString();
    }

    public void IncreaseGold(int amount = 10)
    {
        SetGold(gold + amount);
    }
}

public enum MouseMode
{
    DEFAULT,
    MENU,
    BUILD_TOWER
}