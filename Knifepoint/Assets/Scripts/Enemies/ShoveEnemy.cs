using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveEnemy : EnemyBase
{
    public override IEnumerator FireWeapon()
    {
        Debug.Log("Get Shoved!");
        player.GetShoved(transform.position);
        yield return null;
    }
}
