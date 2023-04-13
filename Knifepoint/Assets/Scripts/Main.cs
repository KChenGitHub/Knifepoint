using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    #region Variables
    [Header("Player")]
    [SerializeField] private Player player;

    [Header("Enemies")]
    /// <summary>
    /// List of enemies in the level that must be defeated to complete the game
    /// </summary>
    public List<EnemyBase> targetEnemies;
    /// <summary>
    /// Empty GameObject in the level that all enemies are children of
    /// </summary>
    public GameObject enemyHolder;
    [SerializeField] private float cleanEnemyListDelay;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private TextMeshProUGUI endGameSummaryText;
    [SerializeField] private GameObject reticle;
    [SerializeField] private GameObject playerStabText;
    [SerializeField] private GameObject RestartButton;
    #endregion

    #region Game Start
    // Start is called before the first frame update
    void Start()
    {
        player.SetMain(this);

        SetEnemyMainRefs();
        CreateEnemyTargetList();
        ChangeTargetEnemyColors();
        GiveEnemiesArmor();
        StartCoroutine(RemoveNullEnemies(cleanEnemyListDelay));


        //For if we restarted
        Time.timeScale = 1f;
        player.GetComponent<FPSController>().lookSpeed = 2;
        player.GetComponent<FPSController>().canMove = true;

        //Lock the Cursor for FPS rotation
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Gives every enemy a reference to the main script.
    /// </summary>
    private void SetEnemyMainRefs()
    {
        foreach (EnemyBase enem in enemyHolder.GetComponentsInChildren<EnemyBase>())
        {
            enem.SetMain(this);
        }
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
    }

    /// <summary>
    /// Changes the colors of all target enemies to the target colors
    /// </summary>
    private void ChangeTargetEnemyColors()
    {
        foreach (EnemyBase enem in targetEnemies)
        {
            enem.isTarget = true;
            enem.ChangeToTargetMat();
        }
    }

    private void GiveEnemiesArmor()
    {
        EnemyBase[] allEnemies = enemyHolder.GetComponentsInChildren<EnemyBase>();
        int listLen = allEnemies.Length;
        int numOfArmorEnemies = (int)Mathf.Floor(listLen / 3);
        for (int i = 0; i < numOfArmorEnemies; i++)
        {
            float randEnemyNum = Random.Range(0, listLen);
            for (int j = 0; j < listLen; j++)
            {
                if (j == randEnemyNum)
                {
                    allEnemies[j].GrantArmor();
                }
            }

        }
    }


    #endregion

    #region Enemy List Management
    /// <summary>
    /// Removes an enemy from the target list once it has been defeated
    /// </summary>
    public void RemoveEnemyFromTargetList(EnemyBase enemy)
    {
        targetEnemies.Remove(enemy);
        if (targetEnemies.Count == 0)
        {
            EndGame(true);
        }
    }

    private IEnumerator RemoveNullEnemies(float removeDelay)
    {
        while (true)
        {
            yield return new WaitForSeconds(removeDelay);
            RemoveNullsFromEnemyList();
        }
    }
    private void RemoveNullsFromEnemyList()
    {
        Debug.Log("Clearing!");
        for (int i = 0; i < targetEnemies.Count; i++)
        {
            if (!targetEnemies[i])
            {
                targetEnemies.RemoveAt(i);
            }
        }
    }


    /// <summary>
    /// Sets the text boxes for when the game ends.
    /// </summary>
    /// <param name="didPlayerWin"></param>
    public void EndGame(bool didPlayerWin = false)
    {
        StopAllCoroutines();
        Time.timeScale = 0;
        reticle.SetActive(false);
        endGameText.enabled = true;
        RestartButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<FPSController>().lookSpeed = 0;
        player.GetComponent<FPSController>().canMove = false;
        if (!didPlayerWin)
        {
            endGameSummaryText.text = $"There were {targetEnemies.Count} targets remaining!";
        }
        endGameSummaryText.enabled = true;
        playerStabText.SetActive(false);
    }

    /// <summary>
    /// Called by the restart button
    /// </summary>
    public void RestartGame()
    {
        Debug.Log("Restart button pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    #endregion

}
