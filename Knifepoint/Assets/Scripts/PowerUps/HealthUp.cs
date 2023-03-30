using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    public int addHealth = 2;

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
        health.currHP += addHealth;

        Destroy(gameObject);

        for (int i = health.maxHP; i > 0; i++)
        {
            if (health.currHP > i)
            {
                health.heartIconList[i + 1].SetActive(true);
            }
        }
    }

    

}
