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

    [Header("Raycast")]
    public LineRenderer laserRend;

    public override IEnumerator FireWeapon()
    {
        laserRend.enabled = true;
        Vector3 firePoint = targetObject.transform.position;
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
        Debug.Log("Firing Weapon!");
        //Raycast
        RaycastHit hit;
        if (Physics.Raycast(attackPoint.transform.position, fireDir, out hit, attackRange))
        {
            hit.collider.gameObject.SetActive(false);
            targetObject = null;
            radarObject = null;
            hasWaited = true;
            navAgent.speed = speed;
        }
    }
}
