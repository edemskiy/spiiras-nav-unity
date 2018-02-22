using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour {
	string id;
    private IEnumerator coroutine;

    float tmpTime = 0;
    Vector3 prevPosition;

    // Use this for initialization
    void Start () {
		id = System.Guid.NewGuid ().ToString("N");
        prevPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator StartTracking(string id, NavMeshController user)
    {
        while (true) {
            if(prevPosition != user.gameObject.transform.position) user.trackUser(id);
            prevPosition = user.gameObject.transform.position;
            tmpTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter (Collider col) {
        if (col.gameObject.GetComponent<NavMeshController> () != null) {
            coroutine = StartTracking(id, col.gameObject.GetComponent<NavMeshController>());
            StartCoroutine(coroutine);
		}
	}

	void OnTriggerExit (Collider col) {
        if (col.gameObject.GetComponent<NavMeshController> () != null) {
            StopCoroutine(coroutine);
        }
	}
}
