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
    private Vector3 prevPosition;

    public GameObject trackers;
    public List<GameObject> trackersList;

    public Path path;
    static int usersCounter = 1;
    public float speed = 5f;

    private int currentWaypoint = 0;

    float coeff;

    List<float> probArr;



    public void Start()
    {
        foreach (Transform child in trackers.transform)
        {
            trackersList.Add(child.gameObject);
        }

        coeff = 3.6f * 59 / (2 * speed);
        // id = System.Guid.NewGuid().ToString("N");
        id = (usersCounter++).ToString();
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();
        seeker.StartPath(transform.position, getNextPosition(), OnPathComplete);
    }

    public void SetProbArr(float[] arr) {
        probArr = new List<float>(arr);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            
            currentWaypoint = 0;
            if(coroutine != null) StopCoroutine(coroutine);
           
            // timer = 0;
            // delayCounter = 0;
            // tmpTime = 0;
           
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
        // tmpTime = 0;
        currentWaypoint++;
    }

    public Vector3 GetPrevPosition()
    {
        return prevPosition;
    }

	public void trackUser(string sensorID){

        System.DateTime startDate = GetComponentInParent<UsersNavController>().GetDateTime(coeff);

        if (startDate.DayOfWeek == System.DayOfWeek.Sunday || startDate.DayOfWeek == System.DayOfWeek.Saturday) return;
        if (startDate.Hour > 18 || startDate.Hour < 10) return;

        // System.IO.FileInfo fi = new System.IO.FileInfo("data/userTracking" + id + ".txt");
        System.IO.FileInfo fi = new System.IO.FileInfo("data/userTracking.txt");
        prevPosition = transform.position;
        ObjectLocation loc = new ObjectLocation(startDate.ToString(), transform.position.x, transform.position.z, this.id);

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
        if (probArr == null) return transform.position;
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