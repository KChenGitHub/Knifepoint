using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastEnemy : EnemyBase
{
    public RaycastEnemy()
    {
        //speed = 6;
        //attackRate = 1.5f;
        //attackRange = 10f;
    }

    [Header("Raycast")]
    public LineRenderer laserRend;

    public override IEnumerator FireWeapon()
    {
        laserRend.enabled = true;
        Vector3 firePoint = GetAttackPoint(); //targetObject.transform.position;
        //Raycasts use DIRECTION and not POSITION so we need to calculate the
        //direction between the target position and the attackPoint
        Vector3 fireDir = firePoint - attackPoint.transform.position;
        while (waitTimer < attackRate)
        {
            waitTimer += Time.deltaTime;
            //attackPoint.transform.position is the GLOBAL position I guess, so
            //we change it to the LOCAL position using InverseTransformPoint.
            //So that means that targetObject should also be a GLOBAL position?
            Vector3[] laserPositions = { transform.InverseTransformPoint(attackPoint.transform.position), transform.InverseTransformPoint(firePoint) };
            //Debug.DrawRay(attackPoint.transform.position, attackPoint.transform.up * attackRange, Color.blue);
            Debug.DrawRay(attackPoint.transform.position, fireDir, Color.blue);
            laserRend.SetPositions(laserPositions);
            //Debug.DrawRay(attackPoint.transform.position, attackPoint.transform.TransformDirection(Vector3.up) * attackRange, Color.red);
            yield return null;
        }
        laserRend.enabled = false;
        waitTimer = 0.0f;
        isWaiting = false;
        //Raycast
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.transform.position, fireDir, out hit, attackRange))
        {
            targetObject = null;
            radarObject = null;
            hasWaited = true;
            //hit player
            if (hit.collider.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(1);
            }

        }

    }

    //I am about to destroy the FUCK out of physics, here we go
    private Vector3 GetAttackPoint()
    {
        var dist = Mathf.Sqrt(attackRange);
        var direction = (targetObject.transform.position - attackPoint.transform.position).normalized * Mathf.Sqrt(attackRange);
        return attackPoint.transform.position + direction * dist;
    }
}
