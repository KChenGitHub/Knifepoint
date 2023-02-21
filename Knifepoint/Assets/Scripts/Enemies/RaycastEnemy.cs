using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEnemy : EnemyBase
{
    public RaycastEnemy()
    {
        speed = 6;
        attackRate = 1.5f;
        attackRange = 10f;
    }

    public override void FireWeapon()
    {
        //Pause before firing weapon
        waitTimer = attackRate;
        if (!hasWaited)
        {
            if (isWaiting)
            {
                waitTimer += Time.deltaTime;
                if (isWaiting = waitTime >= waitTimer)
                {
                    isWaiting = false;
                    hasWaited = true;
                }
            }
            return;
        }

        //base.FireWeapon();
        Debug.Log("Firing Weapon!");
    }
}
