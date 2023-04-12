using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    public int hazardDamage;
    public float timeBetweenDamageTicks;
    public float damageTimer;
    
    

    //fire guyser
    public ParticleSystem firePS;
    public CapsuleCollider guyserCollider;
    private float particleSystemPlaybackTime = 3.3f;
    private float fireGuyserDelayTime = 5.0f;

    //electro fence
    public BoxCollider fenceCollider;
    public GameObject fencePlaneOne, fencePlaneTwo;
    private float electroFenceActiveTime = 4.0f;
    private float electroFenceDelayTime = 6.0f;

    public enum hazardType { acidPit, fireGuyser, electroFence }
    public hazardType type;

    // Start is called before the first frame update
    void Start()
    {
        damageTimer = timeBetweenDamageTicks; //Do this so that the player instantly takes damage on contact with the hazard
        if (type == hazardType.fireGuyser)
        {
            StartCoroutine(GuyserCycle(particleSystemPlaybackTime, fireGuyserDelayTime));
        }
        else if (type == hazardType.electroFence)
        {
            StartCoroutine(FenceCycle(electroFenceActiveTime, electroFenceDelayTime));
        }
    }

    #region fire guyser and electro fence cycles

    private IEnumerator GuyserCycle(float particleSysTime, float guyserDelay)
    {
        if (!firePS || !guyserCollider)
        {
            Debug.LogWarning("Fire Guyser missing either fire particle system or guyser collider!");
            yield return null;
        }
        guyserCollider.enabled = false;
        yield return new WaitForSeconds(guyserDelay);
        while (true)
        {
            firePS.Play();
            yield return new WaitForSeconds(.05f);
            guyserCollider.enabled = true;
            yield return new WaitForSeconds(particleSysTime);
            guyserCollider.enabled = false;
            yield return new WaitForSeconds(guyserDelay);
            
        }
    }

    private IEnumerator FenceCycle(float activeTime, float delayTime)
    {
        if (!fencePlaneOne || !fencePlaneTwo || !fenceCollider)
        {
            Debug.LogWarning("Electric fence is missing at least once fencePlane or collider!");
            yield return null;
        }
        fencePlaneOne.SetActive(false);
        fencePlaneTwo.SetActive(false);
        fenceCollider.enabled = false;
        yield return new WaitForSeconds(delayTime);
        while (true)
        {
            fencePlaneOne.SetActive(true);
            fencePlaneTwo.SetActive(true);
            fenceCollider.enabled = true;
            yield return new WaitForSeconds(activeTime);
            fencePlaneOne.SetActive(false);
            fencePlaneTwo.SetActive(false);
            fenceCollider.enabled = false;
            yield return new WaitForSeconds(delayTime);
        }

    }

    #endregion

    #region Collisions
    /// <summary>
    /// Called as long as the player is touching a hazard to periodically reduce their HP
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Player touch");
        if (other.TryGetComponent<Player>(out Player pc))
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= timeBetweenDamageTicks)
            {
                damageTimer = 0f;
                pc.TakeDamage(hazardDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            enemy.TakeDamage(2); //2 damage defeats the enemies
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            damageTimer = timeBetweenDamageTicks;
        }
    }
    #endregion
}
