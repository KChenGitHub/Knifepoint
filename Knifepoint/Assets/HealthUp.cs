using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    public int addHealth = 6;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
        }
    }

    void Pickup(Collider player)
    {

        Debug.Log("Power up is picked up");
        Player health = player.GetComponent<Player>();
        health.maxHP += addHealth;


        Destroy(gameObject);
    }
}
