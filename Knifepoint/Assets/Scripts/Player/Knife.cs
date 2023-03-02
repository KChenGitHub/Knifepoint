using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public Player player;
    private bool checkingForPlayer = false;
    private float collectDistance = 3f;

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
        yield return new WaitForSeconds(1f);
        checkingForPlayer = true;
    }

    private void PlayerCheck()
    {
        if (checkingForPlayer && Vector3.Distance(transform.position, player.gameObject.transform.position) < collectDistance)
        {
            player.ThrowKnifeReset();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Will add the same for projectile enemies layer
        if (collision.gameObject.TryGetComponent<RaycastEnemy>(out RaycastEnemy enemy))
        {
            enemy.TakeDamage();
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
