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

    public GameObject trackers;
    public List<GameObject> trackersList;

    public Path path;

    public float speed = 5f;

    //public float nextWaypointDistance = 0.8f;

    private int currentWaypoint = 0;

    //public float repathRate = 1000f;
    // private float lastRepath = float.NegativeInfinity;

    float timer = 0.0f;
    float delayCounter = 0;
    float coeff;

    float tmpTime;
    System.DateTime startDate = new System.DateTime(2018, 1, 1, 10, 0, 0);

    List<float> probArr;



    public void Start()
    {
        foreach (Transform child in trackers.transform)
        {
            trackersList.Add(child.gameObject);
        }

        probArr = new List<float>(new float[] { 0.021f, 0.002f, 0.003f, 0.005f, 0.89f, 0.072f, 0.004f, 0.001f, 0.002f });
        coeff = 3.6f * 59 / (2 * speed);
        id = System.Guid.NewGuid().ToString("N");
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        seeker.StartPath(transform.position, getNextPosition(), OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            
            currentWaypoint = 0;
            if(coroutine != null) StopCoroutine(coroutine);
           
            timer = 0;
            delayCounter = 0;
            tmpTime = 0;
           
        }
    }

    IEnumerator StartNewPath(float sec)
    {
        while (true)
        {
            yield return new WaitForSeconds(sec);
            seeker.StartPath(transform.position, getNextPosition(), OnPathComplete);
        }
    }

    public void Update()
    {
        // transform.Translate(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        startDate = startDate.AddSeconds(Time.deltaTime * coeff);
        timer += Time.deltaTime;
        if (path == null) {
            
            return;
        } 
        if (currentWaypoint > path.vectorPath.Count) return;

        if (currentWaypoint == path.vectorPath.Count)
        {
            currentWaypoint++;
            path = null;
            coroutine = StartNewPath(2f);
            StartCoroutine(coroutine);
            return;
        }

        transform.position = new Vector3(path.vectorPath[currentWaypoint].x, transform.position.y, path.vectorPath[currentWaypoint].z);
        tmpTime = 0;
        currentWaypoint++;
    }

	public void trackUser(string sensorID){
        
        if (startDate.DayOfWeek == System.DayOfWeek.Sunday || startDate.DayOfWeek == System.DayOfWeek.Saturday) return;
        if (startDate.Hour > 18 || startDate.Hour < 10) return;

        System.IO.FileInfo fi = new System.IO.FileInfo("data/userTracking.txt");

        ObjectLocation loc = new ObjectLocation(this.startDate.ToString(), transform.position.x, transform.position.z, this.id);

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

    Vector3 getNextPosition()
    {
        float rand = Random.Range(0.0f, 1.0f);
        List<float> sortedProbArr = new List<float>(probArr.ToArray());
        sortedProbArr.Sort();
        int i = 0;
        while (rand > 0)
        {
            rand -= sortedProbArr[i];
            i++;
        }
        return trackersList[probArr.IndexOf(sortedProbArr[i-1])].transform.position;
    }
}

/*
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