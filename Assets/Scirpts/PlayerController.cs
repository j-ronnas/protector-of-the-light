using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CommonCharacterController
{

    MapManager mapManager;
    EnemySpawner enemySpawner;
    TickManager tickManager;

    [SerializeField]
    SpriteRenderer characterSprite;


    Sword sword;

    // Start is called before the first frame update
    public void Init(MapManager mapManager, EnemySpawner enemySpawner )
    {
        this.mapManager = mapManager;
        this.enemySpawner = enemySpawner;
        tickManager = FindAnyObjectByType<TickManager>();
        HurtScreen hs = FindAnyObjectByType<HurtScreen>();
        GetComponent<Health>().AddHurtAction(hs.PlayAnim);
        GetComponent<Health>().AddHurtAction(() => { SoundManager.instance.Play("hurt"); });
        GetComponent<Health>().AddDeathAction(OnDeath);

        sword = GetComponent<Sword>();
        sword.Init(this, mapManager, enemySpawner);
        MoveTo(transform.position, true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TryMove(Vector2.left);
            characterSprite.flipX = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryMove(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TryMove(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TryMove(Vector2.right);
            characterSprite.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            sword.SelectWeapon(GetPos());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(FindAnyObjectByType<Projectile>() == null)
            {
                tickManager.TriggerTick();
            }
        }

        base.Update();
            
    }

    private void TryMove(Vector2 direction)
    {
        Vector2 newPos = GetPos() + direction;
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
