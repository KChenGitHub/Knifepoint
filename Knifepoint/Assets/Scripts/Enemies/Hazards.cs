using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    public int hazardDamage;
    public float timeBetweenDamageTicks;
    public float damageTimer;

    // Start is called before the first frame update
    void Start()
    {
        damageTimer = timeBetweenDamageTicks; //Do this so that the player instantly takes damage on contact with the hazard
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Called as long as the player is touching a hazard to periodically reduce their HP
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player pc))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeBetweenDamageTicks)
            {
                damageTimer = 0f;
                pc.TakeDamage(hazardDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage(2); //2 damage defeats the enemies
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            damageTimer = timeBetweenDamageTicks;
        }
    }
}
