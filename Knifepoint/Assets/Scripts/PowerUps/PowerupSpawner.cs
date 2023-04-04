using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{

    [Header("Spawn Locations")]
    [SerializeField] private PowerupSpawnPoint[] spawnLocations;
    private bool healthPowerupLocationSet, timePowerupLocationSet, knifeAttackPowerupLocationSet = false;

    [Header("Powerup Prefabs")]
    [SerializeField] private GameObject healthPowerup;
    [SerializeField] private GameObject timePowerup;
    [SerializeField] private GameObject knifeAttackPowerup;


    // Start is called before the first frame update
    void Start()
    {
        AssignPowerupLocations();
    }

    /// <summary>
    /// Gets one of the spawn locations randomly
    /// </summary>
    /// <returns></returns>
    private PowerupSpawnPoint chooseRandomLocation()
    {
        int randLocNum = Random.Range(0, spawnLocations.Length);
        for (int i = 0; i < spawnLocations.Length; i++)
        {
            if (i == randLocNum && !spawnLocations[i].isOccupied)
            {
                spawnLocations[i].isOccupied = true;
                return spawnLocations[i];
            }
        }
        return null; //Return 0 if the location is occupied or something else goes wrong
    }

    /// <summary>
    /// Creates one of each powerup at a random location, checking that no powerups spawn in the same location.
    /// </summary>
    private void AssignPowerupLocations()
    {
        while (!healthPowerupLocationSet)
        {
            PowerupSpawnPoint spawnLocation = chooseRandomLocation();
            if (spawnLocation != null)
            {
                Instantiate(healthPowerup, spawnLocation.transform.position, spawnLocation.transform.rotation);
                healthPowerupLocationSet = true;
            }
        }

        while (!timePowerupLocationSet)
        {
            PowerupSpawnPoint spawnLocation = chooseRandomLocation();
            if (spawnLocation)
            {
                Instantiate (timePowerup, spawnLocation.transform.position, spawnLocation.transform.rotation);
                timePowerupLocationSet = true;
            }
        }

        while (!knifeAttackPowerupLocationSet)
        {
            PowerupSpawnPoint spawnLocation = chooseRandomLocation();
            if (spawnLocation)
            {
                Instantiate(knifeAttackPowerup, spawnLocation.transform.position, spawnLocation.transform.rotation);
                knifeAttackPowerupLocationSet = true;
            }
        }
    }
}
