using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected int speed;
    [SerializeField] private NavMeshAgent navAgent;
    private float stoppingDist = 2.0f;
    private Vector3 radarDestination;
    private NavMeshPath currPath;
    public float waitTimer;
    public float waitTime;
    public bool isWaiting;
    public bool hasWaited;

    [Header("Radar")]
    //public bool isRadarClockwise = true;
    public GameObject radarObject;
    public float radarAngle;
    private const float RADARRANGE = 15.0f;
    private const float RADARSPEED = 700.0f;
    //private const float MAXRADARANGLE = 70.0f; //Used if we want a more narrow radar angle

    [Header("Attacks")]
    protected float attackRate;
    protected float attackRange;
    public GameObject targetObject;
    public bool isTargetInRange = false;
    public enum enemyType { racyast, projectile }
    public enemyType type;
    
    // Start is called before the first frame update
    void Start()
    {
        radarDestination = transform.position;
        navAgent.speed = speed;
        navAgent.isStopped = false;
        currPath = new NavMeshPath();
    }

    // Update is called once per frame
    void Update()
    {
        //Radar
        Radar();
        Navigation();
    }

    virtual public void FireWeapon()
    {
        //Overridden by child classes
    }

    /// <summary>
    /// Checks if the enemy is close to its destination.
    /// If it is, waits a small amount of time and then calculates a new destination.
    /// </summary>
    private void Navigation()
    {
        if (radarObject && radarObject.CompareTag("target"))
        {
            navAgent.SetDestination(radarObject.transform.position);
            //IF PLAYER
            targetObject = radarObject;
        }

        if (IsTargetInRange() && !hasWaited) //HASWAITED STARTS FALSE
        {
            if (!isWaiting)
            {
                navAgent.isStopped = true;
                isWaiting = true;
                StartCoroutine(WaitAndFireWeapon());
            }
            return;
        }
        else if (IsTargetInRange())
        {
            hasWaited = false;
        }

        //Checks if the enemy is close to its destination
        if (navAgent.remainingDistance <= stoppingDist)
        {
            //We use the bools hasWaited and isWaiting for the wait timer.
            //hasWaited tracks if the enemy has finished waiting for its CURRENT destination.
            //isWaiting is whether the enemy is CURRENTLY waiting at its destination.
            //I cannot think of a better way of doing this besides redoing the entire
            //logic as a coroutine.
            if (!hasWaited)
            {
                if (!isWaiting)
                {
                    isWaiting = true;
                    StartCoroutine(WaitForNewDestination());
                }
                return;
            }
            
            if (navAgent.CalculatePath(radarDestination, currPath))
            {
                
                navAgent.SetPath(currPath);
                hasWaited = false;
            }
        }
    }

    /// <summary>
    /// Helper method returns whether the target object is within attack range.
    /// </summary>
    /// <returns></returns>
    private bool IsTargetInRange()
    {
        if (targetObject && Vector3.Distance(transform.position, targetObject.transform.position) < attackRange)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Coroutine runs a short timer before calculating a new path for the enemy
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForNewDestination()
    {
        waitTime = Random.Range(0, 5);
        while (waitTimer <= waitTime)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }
        waitTimer = 0.0f; //Reset the timer to 0
        isWaiting = false;
        hasWaited = true;
    }

    /// <summary>
    /// Waits attackRate seconds and then fires the weapon. I'm reusing the timer variables for the wait.
    /// As long as the variables aren't changed the movement and attack logic are paused.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndFireWeapon()
    {
        yield return new WaitForSeconds(attackRate);
        hasWaited = true;
        isWaiting = false;
        FireWeapon();
    }

    /// <summary>
    /// Sends out a raycast that moves in a circle around the enemy
    /// When the raycast collides with certain objects we do stuff
    /// </summary>
    private GameObject Radar()
    {
        //Set variables
        radarAngle += RADARSPEED * Time.deltaTime;
        //Following two lines are OLD for if we only want the enemy to search in a range.
        //radarAngle = isRadarClockwise ? radarAngle + Time.deltaTime * RADARSPEED : radarAngle - Time.deltaTime * RADARSPEED;
        //isRadarClockwise = Mathf.Abs(radarAngle) >= MAXRADARANGLE ? !isRadarClockwise : isRadarClockwise;


        //Raycast
        RaycastHit hit;
        Vector3 heightAdjust = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z); //Add 1 to not be on the floor
        Vector3 otherPos = Quaternion.Euler(0f, radarAngle, 0f) * transform.TransformDirection(Vector3.forward);
        otherPos = otherPos.normalized * RADARRANGE;
        radarDestination = otherPos; //Used for navigation

        Debug.DrawRay(heightAdjust, otherPos, Color.green);
        if (Physics.Raycast(heightAdjust, otherPos, out hit, Mathf.Infinity))
        {
            radarObject = hit.collider.gameObject;
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }

    }


}
