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
        transform.position += speed * Time.deltaTime * transform.up;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(1);
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterTime(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
