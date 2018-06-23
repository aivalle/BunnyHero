using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	public GameObject target;
	private Camera ActualCamera;

	void Start () {
		ActualCamera = Camera.main;
	}

	void FixedUpdate () {
		if (!target) { //Si el objetivo ya no existe entonces destuir este indicador.
			Destroy (gameObject);
		} else {
			PositionArrow ();
		}
	}

	void PositionArrow()
	{
		//Sistema que permite mover el indicador atraves de los bordes de la pantalla:
		transform.localScale = new Vector2(0,0);
		Vector3 Pos = ActualCamera.WorldToViewportPoint(target.transform.position);


		if (Pos.x >= 0.0f && Pos.x <= 1.0f && Pos.y >= 0.0f && Pos.y <= 1.0f) // Si el objetivo es visible entonces no continua apareciendo.
			return;

		transform.localScale = new Vector2(1,1);
		Pos.x -= 0.5f;  
		Pos.y -= 0.5f; 
		Pos.z = 0;

		float fAngle = Mathf.Atan2 (Pos.x, Pos.y);
		transform.localEulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

		Pos.x = 0.5f * Mathf.Sin (fAngle) + 0.5f;  // Place on ellipse touching 
		Pos.y = 0.5f * Mathf.Cos (fAngle) + 0.5f;  //   side of viewport
		Pos.z = ActualCamera.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
		transform.position = ActualCamera.ViewportToWorldPoint(Pos);
	}
}