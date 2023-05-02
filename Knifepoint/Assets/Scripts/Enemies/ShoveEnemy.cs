using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveEnemy : EnemyBase
{
    public override IEnumerator FireWeapon()
    {
        //Lower the y by 1 to make sure the player can get pushed up a little bit.
        Vector3 lowerShoveDir = new Vector3(transform.position.x, transform.position.y - 2f, transform.position.z);
        player.GetShoved(lowerShoveDir);
        //Enemy waits after shoving the player
        navAgent.isStopped = true;
        while (waitTimer < attackRate)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }
        navAgent.isStopped = false;
        waitTimer = 0.0f;
        isWaiting = false;
    }
}
