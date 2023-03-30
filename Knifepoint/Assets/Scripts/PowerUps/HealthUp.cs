using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{
    public int addHealth = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player)) 
        {
            player.increaseHPMax(addHealth);
            Destroy(gameObject);
        }
    }
}
