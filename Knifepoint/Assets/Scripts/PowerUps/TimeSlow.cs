using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    public float timeSpeed = 0.5f; //the in-game speed
    public float duration = 4.5f; //How long the powerup will last
    [SerializeField] private GameObject MeshDisk, MeshCylinder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider other)
    {
        Time.timeScale = timeSpeed;
        MeshDisk.SetActive(false);
        MeshCylinder.SetActive(false);
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
