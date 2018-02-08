using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AboveCamera : MonoBehaviour {
	bool isDragging;
	public bool active;
	public Camera cam;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
		cam.transform.position = new Vector3 (0, 50f, 0);
		cam.orthographicSize = 25;
		cam.transform.LookAt (Vector3.zero);
		cam.gameObject.SetActive (true);
		cam.orthographic = true;
		active = true;
	}

	// Update is called once per frame
	void Update () {
		/* блокировка нажатий сквозь интерфейс */
		if (EventSystem.current.IsPointerOverGameObject () || !active)
			return;
		/**************************************/

		if (Input.GetMouseButtonDown (0)) {
			isDragging = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			isDragging = false;
		}

		Vector3 newPosition = transform.position;
		if (Input.GetKey (KeyCode.Q)) {
			Debug.Log (transform.position.y);
			newPosition.y -= 100f * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.E)) {
			Debug.Log (transform.position.y);
			newPosition.y += 100f * Time.deltaTime;
		}
		if (isDragging) {
			newPosition.x -= Input.GetAxis ("Mouse X");
			newPosition.z -= Input.GetAxis ("Mouse Y");
		}

		transform.position = newPosition;
	}
}