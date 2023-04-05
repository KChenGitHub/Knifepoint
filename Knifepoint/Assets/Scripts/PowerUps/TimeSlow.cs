using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public float timeSpeed = 0.5f; //the in-game speed
    public float duration = 4.5f; //How long the powerup will last

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider other)
    {

        Debug.Log("Power up is picked up");
        Time.timeScale = timeSpeed; //Everything including the player will be affected.

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
