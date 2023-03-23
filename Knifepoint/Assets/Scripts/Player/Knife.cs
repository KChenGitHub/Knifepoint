using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Player player;
    private bool checkingForPlayer = false;
    private float collectDistance = 3f;
    private bool canHurtEnemies = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAfterThrow());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCheck();
    }

    /// <summary>
    /// We wait a short time after the knife is thrown to start checking for the player
    /// because if we start too soon we don't give the knife time to fly away
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAfterThrow()
    {
        yield return new WaitForSeconds(.5f);
        checkingForPlayer = true;
    }

    /// <summary>
    /// Resets the player's knife throw if they're close enough to pick up the knife
    /// </summary>
    private void PlayerCheck()
    {
        if (checkingForPlayer && Vector3.Distance(transform.position, player.gameObject.transform.position) < collectDistance
            || (transform.position.y <= -50f))
        {
            player.ThrowKnifeReset();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Will add the same for projectile enemies layer
        if (canHurtEnemies && other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage();
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        canHurtEnemies = false;
        checkingForPlayer = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        canHurtEnemies = false;
    }
}
