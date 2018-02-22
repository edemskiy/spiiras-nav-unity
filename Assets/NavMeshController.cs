using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour {
    [SerializeField]
    Transform target;

    NavMeshPath path;
    NavMeshAgent navMeshAgent;

	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        setDestination();
	}

    private void setDestination()
    {
        // для демонстрации перемещения к заданной точке
        // navMeshAgent.SetDestination(target.transform.position);
                 
        NavMeshPath pathTest = new NavMeshPath();
        if (navMeshAgent.CalculatePath(target.transform.position, pathTest))
        {
            this.path = pathTest;
            // this.path.corners - массив точек пути; для визуализации.
        }
        
    }

    void Update () {
        
    }
}
