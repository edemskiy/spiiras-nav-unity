using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour {

	private bool fixPosition = false;

	public float mainSpeed = 100.0f; //regular speed
	public float camSens = 0.25f; //How sensitive it with mouse

	private float totalRun= 1.0f;

	private bool isRotating = false; // Angryboy: Can be called by other things (e.g. UI) to see if camera is rotating
	private float speedMultiplier; // Angryboy: Used by Y axis to match the velocity on X/Z axis

	public float mouseSensitivity = 2.0f;        // Mouse rotation sensitivity.
	private float rotationY = -90.0f;

	private Transform cameraTransform;
	private CharacterController _charController;

	void Start(){
		cameraTransform = transform;
		_charController = GetComponent<CharacterController>();
	}


	void Update () {

		/*
		if (EventSystem.current.IsPointerOverGameObject () || fixPosition)
			return;
		*/


		if (Input.GetMouseButtonDown (0)) {
			isRotating = true;
		}
		if (Input.GetMouseButtonUp (0)) {
			isRotating = false;
		}
		if (isRotating) {
			float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * mouseSensitivity;        
			rotationY += Input.GetAxis ("Mouse Y") * mouseSensitivity;
			rotationY = Mathf.Clamp (rotationY, -90, 90);        
			transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0.0f);
		}

		if (Input.GetKey (KeyCode.W)) {
			_charController.Move (cameraTransform.forward);
			// transform.position += cameraTransform.forward;
		}
		if (Input.GetKey (KeyCode.S)) {
			_charController.Move (-cameraTransform.forward);
		}
		if (Input.GetKey (KeyCode.A)) {
			_charController.Move (-cameraTransform.right);
		}
		if (Input.GetKey (KeyCode.D)) {
			_charController.Move (cameraTransform.right);
		}
	}
	public void fixPoition(){
		fixPosition = true;
	}
	public void unfixPoition(){
		fixPosition = false;
	}
}