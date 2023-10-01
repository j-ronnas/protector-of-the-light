using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    PlayerController player;
    Vector2 position;
    TickManager tm;

    // Start is called before the first frame update
    public void Init(Vector2 pos)
    {
        tm = FindAnyObjectByType<TickManager>();
        tm.AddTickAction(OnTick);
        player = FindAnyObjectByType<PlayerController>();
        this.position = pos;
    }

    void OnTick()
    {
        if(position == player.GetPos())
        {
            FindAnyObjectByType<BuildingManager>().IncreaseGold();
            tm.RemoveTickAction(OnTick);
            SoundManager.instance.Play("coin");
            Destroy(gameObject);
        }
    }
}
