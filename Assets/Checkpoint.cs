using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    string id;
    // Use this for initialization
    void Start()
    {
        id = System.Guid.NewGuid().ToString("N");
    }

    // Update is called once per frame
    void Update () {
		
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.GetComponent<UserController> () != null) {
			col.gameObject.GetComponent<UserController> ().trackUser (id);
		}
	}
}
