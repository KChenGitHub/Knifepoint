using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [Header("Enemies")]
    /// <summary>
    /// List of enemies in the level that must be defeated to complete the game
    /// </summary>
    public List<EnemyBase> targetEnemies;
    /// <summary>
    /// Empty GameObject in the level that all enemies are children of
    /// </summary>
    public GameObject enemyHolder; 

    // Start is called before the first frame update
    void Start()
    {
        CreateEnemyTargetList();
        ChangeTargetEnemyColors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Uses the enemyHolder gameobject to change certain enemies into target enemies
    /// </summary>
    private void CreateEnemyTargetList()
    {
        EnemyBase[] allEnemies = enemyHolder.GetComponentsInChildren<EnemyBase>();
        int listLen = allEnemies.Length;
        int numOfTargetEnemies = (int)Mathf.Floor(listLen / 4);

        for (int i = 0; i < numOfTargetEnemies; i++)
        {
            float randEnemyNum = Random.Range(0, listLen);
            for (int j = 0; j < listLen; j++)
            {
                if (j == randEnemyNum)
                {
                    targetEnemies.Add(allEnemies[j]);
                    break;
                }
            }
        }
        Debug.Log($"Enemy List created! There are {numOfTargetEnemies} targets!");
    }

    /// <summary>
    /// Changes the colors of all target enemies to the target colors
    /// </summary>
    private void ChangeTargetEnemyColors()
    {
        foreach (EnemyBase enem in targetEnemies)
        {
            enem.ChangeToTargetMat();
        }
    }

    /// <summary>
    /// Removes an enemy from the target list once it has been defeated
    /// </summary>
    private void RemoveEnemyFromTargetList(EnemyBase enemy)
    {
        targetEnemies.Remove(enemy);
    }

}
