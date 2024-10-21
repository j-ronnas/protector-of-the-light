using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyState{
    CHASING,
    IDLE,
    DEAD,
    ATTACKING
}

public class EnemyController : CommonCharacterController
{
    //Path path;

    MapManager mapManager;
    List<Vector2> path;
    int currentIndex;
    [SerializeField]
    Coin coinPrefab;

    [SerializeField]
    GameObject attackAnim;
    [SerializeField]
    GameObject attackIndicator;

    Dictionary<Vector2, GameObject> attackTargets;
    
    [SerializeField]
    SpriteRenderer spriteRenderer;
    EnemySpawner enemySpawner;
    PlayerController player;

    public bool isMarked;
    EnemyState currentState = EnemyState.CHASING;
    float timeToMove = 1.5f;
    // Start is called before the first frame update
    public void Init(Vector2 startPos, EnemySpawner enemySpawner)
    {
        FindAnyObjectByType<TickManager>().AddTickAction(OnTick);
        mapManager = FindObjectOfType<MapManager>();
        this.enemySpawner = enemySpawner;
        player = FindAnyObjectByType<PlayerController>();
        GetComponent<Health>().AddDeathAction(Die);
        GetComponent<Health>().AddHurtAction(() => { SoundManager.instance.Play("hit"); });
        MoveTo(startPos, true);
        attackTargets = new Dictionary<Vector2, GameObject>();
        currentIndex = -1;
        OnTick();
    }



    private void OnTick()
    {
        if(timeToMove > 0)
        {
            ChangeTimeToMove(-1);
            return;
        }

        isMarked = false;
        switch(currentState){
            case EnemyState.CHASING:
                Chase();
                break;
            case EnemyState.ATTACKING:
                print("attacking Player"); 
                AttackPlayer();
                break;
            default:
            break;
        }

        spriteRenderer.color = timeToMove <= 0 ? Color.white : Color.gray;
        
       
    }

    private void ChangeTimeToMove(float amount){
        timeToMove += amount;
        if(timeToMove <= 0){
            //We can move next turn
            spriteRenderer.color = Color.white;
            ReadyMove();
        }else{
            spriteRenderer.color = Color.gray;
        }

    }

    private void Chase(){
        if (Vector2.SqrMagnitude(player.GetPos()-GetPos()) <= 1){
            ReadyMove();
            return;
        }

        ChangeTimeToMove(1.5f);

        GetPathToPlayer();

        currentIndex++;
        
        
        if( currentIndex < path.Count)
        {
            Vector2 targetPos = path[currentIndex];
            MoveTo(targetPos);
        }else
        {
            Debug.Log("Enemy reached end");
            currentState = EnemyState.IDLE;
            // we reached the end           
        }


        
    }

    private void GetPathToPlayer(){
        path = mapManager.findPath(GetPos(), player.GetPos());
        currentIndex = 0;
    }

    private void ClearAllTargets(){
        foreach(GameObject target in attackTargets.Values){
            Destroy(target);
        }
        
    }

    private void ReadyMove(){
        if (Vector2.SqrMagnitude(player.GetPos()-GetPos()) <= 1){
            ClearAllTargets();
            attackTargets.Clear();
            foreach(Vector2 v in mapManager.GetTilesInRange(GetPos(), 1)){
                if(v == GetPos()){
                    continue;
                }
                attackTargets.Add(v, Instantiate(attackIndicator, v, Quaternion.identity));
            }
            currentState = EnemyState.ATTACKING;
            return;
        }
        //Indicator for move?
        currentState = EnemyState.CHASING;
        
    }
    private void AttackPlayer(){
        ClearAllTargets();
        if(attackTargets.ContainsKey(player.GetPos())){
            Instantiate(attackAnim, player.GetPos(), Quaternion.identity);
            player.GetComponent<Health>().Hurt();
            ChangeTimeToMove(1.5f);
        }
        else{
            Chase();
        }
        
    }

    public void Die()
    {
        Coin c = Instantiate(coinPrefab, GetPos(), Quaternion.identity, enemySpawner.transform);
        c.Init(GetPos());
        isMarked = true;
        FindAnyObjectByType<TickManager>().RemoveTickAction(OnTick);
        enemySpawner.OnEnemyDeath(this);
        Destroy(gameObject);
    }
}
