using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	//Este script es aplicado a la MainCamera del juego con el conejo.

	public static CameraMovement CameraM;
	private float dampTime ;
	public Transform targetP;
	public bool Follow;

	private Vector3 velocity = Vector3.zero;
	private Camera ActualCamera;

	void Awake(){
		CameraM = this;
	}

	void Start(){
		Follow = true;
		ActualCamera = Camera.main;
		transform.position = new Vector3(targetP.position.x + 10.0f,targetP.position.y + 5.0f, transform.position.z);
	}

	void FixedUpdate()
	{
		if (GameManager.GameM.RunGame || GameManager.GameM.EndGame) { //Si el juego esta en curso entonces el movimiento de la camara tiene un retroseso menor.
			dampTime = 0.1f;
		} else {
			dampTime = 2.5f;
		}

		if (targetP) {
			if(Follow){
				//Sistema que permite el movimiento de la camara al objetivo dado:
				Vector3 aheadPoint = targetP.localPosition + new Vector3 (10f, 0, 0);
				Vector3 point = ActualCamera.WorldToViewportPoint (aheadPoint);
				Vector3 delta = aheadPoint - ActualCamera.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z));
				Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
			}
		}
	}
}
