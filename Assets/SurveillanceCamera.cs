using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour {
	string id;
    private IEnumerator coroutine;

    float tmpTime = 0;

    // Use this for initialization
    void Start () {
		id = System.Guid.NewGuid ().ToString("N");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator StartTracking(string id, UserController user)
    {
        while (true) {
            // Debug.Log(Time.realtimeSinceStartup - tmpTime);
            user.trackUser(id);
            tmpTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter (Collider col) {
        if (col.gameObject.GetComponent<UserController> () != null) {
            
            coroutine = StartTracking(id, col.gameObject.GetComponent<UserController>());
            StartCoroutine(coroutine);
		}
	}

	void OnTriggerExit (Collider col) {
        if (col.gameObject.GetComponent<UserController> () != null) {
            
            StopCoroutine(coroutine);
        }
	}
}
