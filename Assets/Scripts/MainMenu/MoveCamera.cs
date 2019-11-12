using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	private Vector3 ResetCamera; // original camera position
	private Vector3 Origin; // place where mouse is first pressed
	private Vector3 Diference; // change in position of mouse relative to origin
	public float MinDistanceX;
	public float MaxDistanceX;
	public bool Stop;
	public float ActualPositionX;
	void Start()
	{
		ResetCamera = Camera.main.transform.position;
		ActualPositionX = transform.position.x;
	}
		

	void Update(){
		

	}

	void LateUpdate()
	{
		ActualPositionX = transform.position.x;
		if (ActualPositionX >= MaxDistanceX || ActualPositionX  <= MinDistanceX) {
			Stop = true;
		} else {
			Stop = false;
		}
	
		if(Input.GetMouseButtonDown(0))
		{
			Origin = MousePos();
		}
		if (Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ())
		{
			
				Diference = MousePos() - transform.position;

				transform.position = new Vector3(Origin.x - Diference.x, 0,-10f);

		}
		if (Input.GetMouseButton(1)) // reset camera to original position
		{
			transform.position = ResetCamera;
		}

		if(Stop && ActualPositionX < MinDistanceX){
			float newX = Mathf.Lerp (transform.position.x, MinDistanceX + 0.1f, Time.deltaTime * 10f);
			Vector3 newPosition = new Vector3 (newX, 0,-10f);
			transform.position = newPosition;
			Stop = false;
		}
		if (Stop && ActualPositionX > MaxDistanceX){
			float newX = Mathf.Lerp (transform.position.x, MaxDistanceX - 0.1f, Time.deltaTime * 10f);
			Vector3 newPosition = new Vector3 (newX, 0,-10f);
			transform.position = newPosition;
			Stop = false;
		}
	}
	// return the position of the mouse in world coordinates (helper method)
	Vector3 MousePos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
