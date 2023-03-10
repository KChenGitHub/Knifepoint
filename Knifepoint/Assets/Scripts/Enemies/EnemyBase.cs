using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public Main main;
    public bool isTarget = false;
    
    [Header("Health")]
    public int HP = 2;
    public bool hasArmor = false;
    private bool AmIDead = false;

    [Header("Materials")]
    [SerializeField] private Material targetMat;
    [SerializeField] private MeshRenderer meshRend;

    [Header("Movement")]
    [SerializeField] protected int speed;
    [SerializeField] protected NavMeshAgent navAgent;
    private float stoppingDist = 2.0f;
    private float rotationSpeed = 15f;
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
    private const float RADARRANGE = 20.0f;
    private const float RADARSPEED = 700.0f;
    //private const float MAXRADARANGLE = 70.0f; //Used if we want a more narrow radar angle

    [Header("Attacks")]
    [SerializeField] protected float attackRate;
    [SerializeField] protected float attackRange;
    [SerializeField] protected GameObject targetObject;
    [SerializeField] protected GameObject attackPoint;
    //public bool isTargetInRange = false;
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
        KillSwitch();
    }

    #region References and Materials
    /// <summary>
    /// Changes the material to the target colors. This is called from the main at start.
    /// </summary>
    public void ChangeToTargetMat()
    {
        meshRend.material = targetMat;
    }

    /// <summary>
    /// Gives the enemy script a reference to the main script. This is called from the main at start.
    /// </summary>
    /// <param name="mainRef"></param>
    public void SetMain(Main mainRef)
    {
        main = mainRef;
    }
    #endregion

    #region Movement and Targeting
    virtual public IEnumerator FireWeapon()
    {
        //Overridden by child classes
        yield return null;
    }

    /// <summary>
    /// Checks if the enemy is close to its destination.
    /// If it is, waits a small amount of time and then calculates a new destination.
    /// </summary>
    private void Navigation()
    {
        if (radarObject && radarObject.TryGetComponent<Player>(out Player player))
        {
            navAgent.SetDestination(radarObject.transform.position);
            targetObject = radarObject;
        }

        if (IsTargetInRange())
        {
            navAgent.speed = 0;
            if (!isWaiting && IsFacingTarget())
            {
                
                isWaiting = true;
                hasWaited = false;
                StartCoroutine(FireWeapon());
            } //if isWaiting then the enemy should be in the fireweapon coroutine
            else if (!isWaiting)
            {
                RotateTowards(targetObject.transform);
            }
            return;
        }

        navAgent.speed = speed; //Reset speed if the enemy isn't close to its target
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
        }

        //Calculate a new path if we haven't exited the function yet
        if (navAgent.CalculatePath(radarDestination, currPath) && hasWaited)
        {
            navAgent.SetPath(currPath);
            hasWaited = false;
        }
    }

    /// <summary>
    /// Shamelessly stolen from https://answers.unity.com/questions/540120/how-do-you-update-navmesh-rotation-after-stopping.html
    /// </summary>
    /// <param name="target"></param>
    private void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
    /// Uses the dot product along with the targetObject to see if the enemy is
    /// "facing" the target (dot product = 1 when facing target)
    /// </summary>
    /// <returns></returns>
    private bool IsFacingTarget()
    {
        if (targetObject && Vector3.Dot(transform.forward, (targetObject.transform.position - transform.position).normalized) >= .988f)
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
        int waitTime = Random.Range(0, 5);
        yield return new WaitForSeconds(waitTime);
        waitTimer = 0.0f; //Reset the timer to 0
        isWaiting = false;
        hasWaited = true;
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
        //Vector3 heightAdjust = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z); //Add 1 to not be on the floor
        Vector3 otherPos = Quaternion.Euler(0f, radarAngle, 0f) * transform.TransformDirection(Vector3.forward);
        otherPos = otherPos.normalized * RADARRANGE;
        radarDestination = otherPos; //Used for navigation

        Debug.DrawRay(transform.position, otherPos, Color.green);
        if (Physics.Raycast(transform.position, otherPos, out hit, RADARRANGE))
        {
            radarObject = hit.collider.gameObject;
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }

    }
    #endregion

    #region HP and Armor
    /// <summary>
    /// Reduces enemy HP and destroys it if HP reaches 0.
    /// Also accounts for enemy armor.
    /// </summary>
    /// <param name="damage">Damage is defaulted to 2 for the knife. When using the first, we put 1 for this value.
    /// The 1 for the fist includes that the fist cannot damage enemy armor.</param>
    public void TakeDamage(int damage = 2)
    {
        if (!hasArmor)
        {
            HP -= damage;
        }
        else if (damage > 1)
        {
            hasArmor = false;
        }

        if (HP <= 0)
        {
            if (isTarget)
            {
                main.RemoveEnemyFromTargetList(this);
            }
            AmIDead = true;
        }
    }

    /// <summary>
    /// This function exists to delay the destruction of the enemy for a frame
    /// so that the main can remove it from the target list first.
    /// </summary>
    private void KillSwitch()
    {
        if (AmIDead)
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
