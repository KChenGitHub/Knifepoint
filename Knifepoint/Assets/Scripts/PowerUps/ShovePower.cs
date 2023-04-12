using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovePower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            player.GiveShovePower();
            Destroy(gameObject);
        }
    }
}
