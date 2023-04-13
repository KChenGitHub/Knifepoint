using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private readonly float speed = 10f;
    public Vector3 dir;

    void Start()
    {
        StartCoroutine(DestroyAfterTime(5.0f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed * Time.deltaTime * dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(1);
        }
        else if (collision.collider.TryGetComponent<EnemyBase>(out _))
        {
            return;
        }
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
