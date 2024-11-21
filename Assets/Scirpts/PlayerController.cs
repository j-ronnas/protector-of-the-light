using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CommonCharacterController
{

    MapManager mapManager;
    EnemyManager enemySpawner;
    TickManager tickManager;

    [SerializeField]
    SpriteRenderer characterSprite;


    Weapon[] weapons;
    int selectedWeapon = -1;

    // Start is called before the first frame update
    public void Init(MapManager mapManager, EnemyManager enemySpawner )
    {
        this.mapManager = mapManager;
        this.enemySpawner = enemySpawner;
        tickManager = FindAnyObjectByType<TickManager>();
        HurtScreen hs = FindAnyObjectByType<HurtScreen>();
        GetComponent<Health>().AddHurtAction(hs.PlayAnim);
        GetComponent<Health>().AddHurtAction(() => { SoundManager.instance.Play("hurt"); });
        GetComponent<Health>().AddDeathAction(OnDeath);

        weapons = GetComponentsInChildren<Weapon>();
        foreach(Weapon w in weapons){
            w.Init(mapManager, enemySpawner);
        }
        
        MoveTo(Vector2Int.FloorToInt(transform.position), true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(Vector2Int.left);
            characterSprite.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(Vector2Int.right);
            characterSprite.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(FindAnyObjectByType<Projectile>() == null)
            {
                tickManager.TriggerTick();
            }
        }

        Weapons();

        base.Update();
            
    }

    private void Weapons(){


        if (Input.GetKeyDown(KeyCode.E) && weapons.Length > 1)
        {
            weapons[1].ClearIndicators();
            weapons[1].enabled = false;

            weapons[0].enabled = true;
            weapons[0].SelectWeapon(GetPos());
        }
        if (Input.GetKeyDown(KeyCode.Q) && weapons.Length > 1)
        {
            weapons[0].ClearIndicators();
            weapons[0].enabled = false;

            weapons[1].enabled = true;
            weapons[1].SelectWeapon(GetPos());
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int newPos = GetPos() + direction;
        if (!mapManager.CanMove(newPos) || FindAnyObjectByType<Projectile>() != null)
        {
            return;
        }
        else
        {
            MoveTo(newPos);
            tickManager.TriggerTick();
        }
    }



    private void OnDeath()
    {
        FindAnyObjectByType<GameManager>().GameOver();
    }
}
