using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    private float timeSpeed = 0.5f; //the in-game speed
    private float duration = 2.5f; //How long the powerup will last
    [SerializeField] private GameObject MeshDisk, MeshCylinder;
    public GameObject timeSlowPanel;

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
        timeSlowPanel.SetActive(true);
        yield return new WaitForSeconds(duration); //Actually 5 seconds because of half time
        timeSlowPanel.SetActive(false);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
