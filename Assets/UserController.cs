using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserController : MonoBehaviour {
    //static readonly Random getrandom = new Random();
    static int usersCounter = 1;
    string id;

    public GameObject trackers;
    public List<GameObject> trackersList;

    Vector3 prevPosition;
    //NavMeshPath path;
    NavMeshAgent navMeshAgent;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        id = (usersCounter++).ToString();
        foreach (Transform child in trackers.transform)
        {
            trackersList.Add(child.gameObject);
        }
	}

    private void setDestination()
    {
        /* 
        NavMeshPath pathTest = new NavMeshPath();
        if (navMeshAgent.CalculatePath(transform.position, pathTest))
        {
            this.path = pathTest;
            // this.path.corners - массив точек пути; для визуализации.
        }
        */
        
    }

    void Update () {
        if (!navMeshAgent.hasPath)
        {
            // WaitForSeconds(UnityEngine.Random.Range(15.0f, 150.0f) * 1000);
            if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.9)
            {
                navMeshAgent.SetDestination(trackersList[UnityEngine.Random.Range(1, trackersList.Count)].transform.position);
            }
            else
            {
                navMeshAgent.SetDestination(trackersList[0].transform.position);
            }

        }
    }

    public Vector3 GetPrevPosition()
    {
        return prevPosition;
    }

    public void trackUser(string sensorID)
    {
        System.DateTime startDate = GetComponentInParent<UsersNavController>().GetDateTime();

        if (startDate.DayOfWeek == System.DayOfWeek.Sunday || startDate.DayOfWeek == System.DayOfWeek.Saturday) return;
        if (startDate.Hour > 18 || startDate.Hour < 10) return;

        System.IO.FileInfo fi = new System.IO.FileInfo("data/userTracking.txt");
        prevPosition = transform.position;
        ObjectLocation loc = new ObjectLocation(startDate.ToString(), transform.position.x, transform.position.z, this.id);

        if (!fi.Exists)
        {
            using (System.IO.StreamWriter sw = fi.CreateText())
            {
                sw.WriteLine(loc.toJSON());
            }
        }
        else
        {
            using (System.IO.StreamWriter sw = fi.AppendText())
            {
                sw.WriteLine(loc.toJSON());
            }
        }
    }
}
