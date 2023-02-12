using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private int speed;
    [SerializeField] private NavMeshAgent navAgent;
    public GameObject navP1, navP2;
    private float stoppingDist = 2.0f;
    private Vector3 destination;
    private bool destinationReached = false;

    [Header("Radar")]
    public bool isRadarClockwise = true;
    public float radarAngle;
    private const float RADARRANGE = 7.0f;
    private const float RADARSPEED = 200.0f;
    private const float MAXRADARANGLE = 70.0f;

    [Header("Attacks")]
    [SerializeField] private float attackRate;
    private enum enemyType { racyast, projectile }
    private enemyType type;
    
    // Start is called before the first frame update
    void Start()
    {
        destination = navP1.transform.position;
        navAgent.speed = speed;
        navAgent.SetDestination(navP1.transform.position);
        navAgent.isStopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Radar
        Radar();

        //Temp Navigation
        if (Vector3.Distance(transform.position, destination) < stoppingDist)
        {
            destinationReached = true;
        }
        if (destinationReached)
        {
            navAgent.SetDestination(navP2.transform.position);
            destinationReached = false;
        }
    }

    private void FindNewDestination()
    {

    }

    /// <summary>
    /// Sends out a raycast in front of the enemy every frame.
    /// This raycast moves in an area in front of the enemy.
    /// The raycast adjusts its angle every frame, moving back
    /// and forth in a 140 degree cone.
    /// </summary>
    private void Radar()
    {
        //Set variables
        radarAngle = isRadarClockwise ? radarAngle + Time.deltaTime * RADARSPEED : radarAngle - Time.deltaTime * RADARSPEED;
        isRadarClockwise = Mathf.Abs(radarAngle) >= MAXRADARANGLE ? !isRadarClockwise : isRadarClockwise;

        //Raycast
        RaycastHit hit;
        Vector3 heightAdjust = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z); //Add 1 to not be on the floor
        Vector3 otherPos = Quaternion.Euler(0f, radarAngle, 0f) * transform.TransformDirection(Vector3.forward);
        otherPos = otherPos.normalized * RADARRANGE;
        Debug.DrawRay(heightAdjust, otherPos, Color.green);
        if (Physics.Raycast(transform.position, otherPos, out hit, Mathf.Infinity))
        {
            //Actual Raycast code here
        }
    }

}
