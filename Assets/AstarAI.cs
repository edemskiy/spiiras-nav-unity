using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    float timer = 0;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Debug.Log(p.GetTotalLength());
            timer = 0;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

        }

        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count)
        {
            //Debug.Log(Time.realtimeSinceStartup);
            
            currentWaypoint++;
            // Debug.Log(path.GetTotalLength() / (timer*speed));
            Debug.Log(timer);
            if (targetPosition.position != new Vector3(16f, 0.0f, -15f))
            {
                targetPosition.position = new Vector3(16f, 0.0f, -15f);
            }
            else
            {
                targetPosition.position = new Vector3(0f, 0f, -12f);
            }
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            return;
        }

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        //Debug.Log(path.vectorPath[currentWaypoint]);
        //Debug.Log(dir);
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed;
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        controller.SimpleMove(velocity);
        if (currentWaypoint == 0)
        {
            // logTime = Time.realtimeSinceStartup;
            // Debug.Log(logTime);
        }

        // The commented line is equivalent to the one below, but the one that is used
        // is slightly faster since it does not have to calculate a square root
        //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
        if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }
}