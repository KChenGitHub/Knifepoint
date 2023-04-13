using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : EnemyBase
{

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject attackIndicator; //GameObject which is visible when the enemy is about to attack

    public override IEnumerator FireWeapon()
    {
        //wait
        attackIndicator.SetActive(true);

        while (waitTimer < attackRate)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        //fire
        GameObject projectile = Instantiate(projectilePrefab, attackPoint.transform.position, attackPoint.transform.rotation);
        projectile.GetComponent<EnemyBullet>().dir = attackPoint.transform.up;
        waitTimer = 0.0f;
        isWaiting = false;
        attackIndicator.SetActive(false);

    }

}
