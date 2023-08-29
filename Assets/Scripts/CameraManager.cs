using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public Camera cam;
	public Transform camPivot;

	public void Start(){
		cam = GameObject.Find("Camera").GetComponent<Camera>();	
		camPivot = GameObject.Find("CamPivot").transform;
	}
	
	bool cameraMovementClicksPermitted = false;
	bool leftClickHeld = false;
	bool rightClickHeld = false;
	Vector3 oldMousePosition = new Vector3(0,0);
	float oldMouseXForRotation = 0;
	float leftClickLastTime = 0;
	Vector2 camVelocityAtPointOfLettingGo = new Vector2(0,0);
	
	Vector2 cameraVelocity = new Vector2(0,0);
	
	private float CAM_MIN_Y = 36f;
	private float CAM_MAX_Y = 66f;
	
	private float CAM_MIN_Z = -25.7f;
	private float CAM_MAX_Z = -133.3f;
	
	private float CAM_MIN_ROTATION_X = 51.53f;
	private float CAM_MAX_ROTATION_X = 22.7f;
	
	private float MAX_CAMERA_LOCATION_DIFF = 0.05f;	
	
	private float scrollAmount = Mathf.PI/4f; //-PI/2 to PI/2

	public void Update(){
		
		//movement
		
		if (cameraMovementClicksPermitted){
			if (Input.GetMouseButtonDown(0)){
				oldMousePosition = Input.mousePosition;
				leftClickHeld = true;				
			}
		}
		
		if (Input.GetMouseButtonUp(0)){
			oldMousePosition = Input.mousePosition;
			leftClickHeld = false;
			leftClickLastTime = Time.time;
		}
		
		if (leftClickHeld){
			Vector2 diff = Input.mousePosition- oldMousePosition;
			
			diff.x = Mathf.Clamp(diff.x, -MAX_CAMERA_LOCATION_DIFF, MAX_CAMERA_LOCATION_DIFF);
			diff.y = Mathf.Clamp(diff.y, -MAX_CAMERA_LOCATION_DIFF, MAX_CAMERA_LOCATION_DIFF);			
			
			//here's what you can do: apply the 2D mouse movement to the campivot based on the current yaw angle of the campivot - that way, you circumvent ever having to project to world space.

		}
		else if (rightClickHeld){ 
			camPivot.Rotate(0, Input.mousePosition.x - oldMouseXForRotation, 0, Space.Self);
			oldMouseXForRotation = Input.mousePosition.x;
			cameraVelocity = new Vector2(0,0);
		} else {
			
		}
		
		//rotation. The 'rightClickHeld' check has been moved into the if else statement above, because it leads to disaster if conducted in the same frame as leftClickHeld
		
		if (cameraMovementClicksPermitted){
			if (Input.GetMouseButtonDown(1)){
				oldMouseXForRotation = Input.mousePosition.x;
				rightClickHeld = true;			
			}
		}
		
		if (Input.GetMouseButtonUp(1)){
			oldMouseXForRotation = Input.mousePosition.x;
			rightClickHeld = false;			
		}
		
		float scrollDelta = -Input.mouseScrollDelta.y * 0.25f;
		
		if (scrollDelta > 0 && (scrollAmount < Mathf.PI/2f)){
			scrollAmount += scrollDelta;
		} else if (scrollDelta < 0 && (scrollAmount > -Mathf.PI/2f)){
			scrollAmount += scrollDelta; //it says +=, as there is no need to overaccommodate; the negative will sort itself out
		} 
		
		float zeroToOneClamped = (Mathf.Sin(scrollAmount)/2f) + 0.5f;
		
		cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, CAM_MIN_Y + ((CAM_MAX_Y - CAM_MIN_Y)*zeroToOneClamped), CAM_MIN_Z + ((CAM_MAX_Z - CAM_MIN_Z)*zeroToOneClamped));
		cam.transform.localEulerAngles = new Vector3(CAM_MIN_ROTATION_X + ((CAM_MAX_ROTATION_X - CAM_MIN_ROTATION_X)*zeroToOneClamped), cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
	}
	
	public void OnMouseEnter(){ //mouse has entered a region of the screen where camera movement clicks are permitted
		cameraMovementClicksPermitted = true;	
	}
	
	public void OnMouseExit(){	
		cameraMovementClicksPermitted = false;
	}
}