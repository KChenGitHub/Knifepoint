using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSwarm : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent<Player>(out Player player))
        {
            player.GiveKnifeSwarm();
            Destroy(gameObject);
        }
    }
}
