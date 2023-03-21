using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 20f;
    public Vector3 dir;
    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * transform.up;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Enemy projectile hit!");
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("Enemy projectile hit player!");
            player.TakeDamage(1);
        }

        Destroy(gameObject);
    }
}
