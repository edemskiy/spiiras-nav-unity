using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UserController : MonoBehaviour {

    public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;
    private IEnumerator coroutine;
    private string id;

    public Path path;

    public float speed = 5f;

    //public float nextWaypointDistance = 0.8f;

    private int currentWaypoint = 0;

    //public float repathRate = 1000f;
    private float lastRepath = float.NegativeInfinity;

    float timer = 0.0f;
    float stepTime;
    float delayCounter = 0;

    float tmpTime;
    System.DateTime startDate = new System.DateTime(2018, 1, 1, 10, 0, 0);


    public void Start()
    {
        id = System.Guid.NewGuid().ToString("N");
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        
        if (!p.error)
        {
            path = p;
            
            currentWaypoint = 0;
            // stepTime = p.GetTotalLength() / (speed * path.vectorPath.Count);
            //Debug.Log(stepTime);
            // coroutine = StartMoving(0);

            timer = 0;
            delayCounter = 0;
            tmpTime = 0;
            // StartCoroutine(coroutine);
        }
    }

    public void Update()
    {
        // transform.Translate(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        
        
        timer += Time.deltaTime;
        // Debug.Log(1/Time.deltaTime);
        if (path == null) return;
        if (currentWaypoint > path.vectorPath.Count) return;

        if (currentWaypoint == path.vectorPath.Count)
        {
            currentWaypoint++;
            Debug.Log(path.GetTotalLength() / timer);
            if (targetPosition.position != new Vector3(29f, 0.5f, -20f))
            {
                targetPosition.position = new Vector3(29f, 0.5f, -20f);
            }
            else
            {
                targetPosition.position = new Vector3(10f, 0.5f, 14f);
            }
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            return;
        }

        transform.position = new Vector3(path.vectorPath[currentWaypoint].x, transform.position.y, path.vectorPath[currentWaypoint].z);
        delayCounter += (tmpTime - stepTime);
        tmpTime = 0;
        currentWaypoint++;
    }

	public void trackUser(string sensorID){

        // 59-60 fps??
        float k = 3.6f * 59 / (2 * speed);

        System.IO.FileInfo fi = new System.IO.FileInfo("data/userTracking.txt");

        // ниже Time.realtimeSinceStartup необходимо умножать "k" (увеличение скорости)
        ObjectLocation loc = new ObjectLocation(this.startDate.AddSeconds(Time.realtimeSinceStartup * k).ToString(), transform.position.x, transform.position.z, this.id);

        if (!fi.Exists) {
            using (System.IO.StreamWriter sw = fi.CreateText()) {
                sw.WriteLine(loc.toJSON());
            }
        }
        else { 
            using (System.IO.StreamWriter sw = fi.AppendText()) { 
		        sw.WriteLine(loc.toJSON());
            }
        }
    }
}

/*
   IEnumerator StartMoving(float sec) {
       while (true) {
           Move();
           yield return new WaitForSeconds(sec);

       }
   }

   void Move() {
       if (path == null) return;
       if (currentWaypoint > path.vectorPath.Count) return;

       if (currentWaypoint == path.vectorPath.Count)
       {
           currentWaypoint++;
           StopCoroutine(coroutine);
           Debug.Log(path.GetTotalLength() / timer);
           if(targetPosition.position != new Vector3(16f, 0.0f, -15f))
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

       transform.position = new Vector3(path.vectorPath[currentWaypoint].x, transform.position.y, path.vectorPath[currentWaypoint].z) ;
       delayCounter += (tmpTime - stepTime);
       tmpTime = 0;
       currentWaypoint++;
   }
   */
