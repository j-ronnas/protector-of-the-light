using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeapon : Weapon
{
    int range = 2;

    public override void SelectWeapon(Vector2 position)
    {
        availableTiles.Clear();
        foreach(Vector2 v in mapManager.GetTilesInRange(position, range)){
            availableTiles.Add(new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y) ), Instantiate(tileIndicator, v, Quaternion.identity));
        }
    }

}
