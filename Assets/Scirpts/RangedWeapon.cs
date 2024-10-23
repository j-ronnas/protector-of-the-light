using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    int range = 5;

    public override void SelectWeapon(Vector2 position)
    {
        availableTiles.Clear();
        Vector2[] directions = {
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(-1,1),
            new Vector2(-1,0),
            new Vector2(-1,-1),
            new Vector2(0,-1),
            new Vector2(1,-1),
        };

        foreach(Vector2 dir in directions){
            foreach(Vector2 v in mapManager.GetTilesFromRay(position, dir, range, 0.05f)){
                availableTiles.TryAdd(new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y) ), Instantiate(tileIndicator, v, Quaternion.identity));
            }
        }
        
    }
}
