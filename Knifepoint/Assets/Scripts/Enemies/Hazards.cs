using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hazards : MonoBehaviour
{
    public int hazardDamage;
    public float timeBetweenDamageTicks;
    public float damageTimer;
    public NavMeshObstacle navObstacle;
    public ParticleSystem startupParticles, activeParticles, startupParticles2, activeParticles2;

    //fire guyser
    public CapsuleCollider guyserCollider;
    private float particleSystemPlaybackTime = 3.0f;
    private float fireGuyserDelayTime = 5.0f;

    //electro fence
    public BoxCollider fenceCollider;
    private float electroFenceActiveTime = 2.5f;
    private float electroFenceDelayTime = 2.5f;
    


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
        if (!activeParticles || !startupParticles || !guyserCollider || !navObstacle)
        {
            Debug.LogWarning("Fire Guyser missing a component!");
            yield break;
        }
        guyserCollider.enabled = false;
        navObstacle.enabled = false;
        yield return new WaitForSeconds(guyserDelay);

        while (true)
        {
            //Startup
            startupParticles.Play();
            yield return new WaitForSeconds(3f);

            //Particles
            activeParticles.Play();
            guyserCollider.enabled = true;
            navObstacle.enabled = true;
            yield return new WaitForSeconds(particleSysTime);
            guyserCollider.enabled = false;
            navObstacle.enabled = false;
            yield return new WaitForSeconds(guyserDelay);
            
        }
    }

    private IEnumerator FenceCycle(float activeTime, float delayTime)
    {
        if (!activeParticles || !activeParticles2 || !startupParticles || !startupParticles2 || !fenceCollider || !navObstacle)
        {
            Debug.LogWarning("Electric fence is missing a component!");
            yield break;
        }
        fenceCollider.enabled = false;
        navObstacle.enabled = false;
        yield return new WaitForSeconds(delayTime);
        while (true)
        {
            //Startup
            startupParticles.Play();
            startupParticles2.Play();
            yield return new WaitForSeconds(3f);

            //Active Particles
            activeParticles.Play();
            activeParticles2.Play();
            fenceCollider.enabled = true;
            navObstacle.enabled = true;
            yield return new WaitForSeconds(activeTime);
            activeParticles.Stop();
            activeParticles2.Stop();
            fenceCollider.enabled = false;
            navObstacle.enabled = false;
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
