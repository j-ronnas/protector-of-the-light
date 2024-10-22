using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    int range = 5;

    public override void SelectWeapon(Vector2 position)
    {
        availableTiles.Clear();
        foreach(Vector2 v in mapManager.GetTilesFromRay(position, new Vector2(1,-1), range, 0.05f)){
            availableTiles.Add(new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y) ), Instantiate(tileIndicator, v, Quaternion.identity));
        }
    }
}
