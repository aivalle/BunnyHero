using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

	//Este script es usado para el lanzamiento de ataque del conejo hacia los misiles próximos (similar al MissilesManager).

	public GameObject target;
	private float speed = 5f;
	private float rotateSpeed = 200f;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate () {
		if (!target) { //Si el objetivo no esta "vivo" o activo.
			DeleteMissile();
		} else {
			//Persigue al objetivo:
			Vector2 direction = (Vector2)target.transform.position - rb.position;
			direction.Normalize ();
			float rotateAmount = Vector3.Cross (direction, transform.up).z;
			rb.angularVelocity = -rotateAmount * rotateSpeed;
			rb.velocity = transform.up * speed;

		}
	}

	void DeleteMissile(){
		Destroy (gameObject);
	}


	void OnTriggerEnter2D (Collider2D collider)
	{
		//Detecta que la colisión sea un misil para que el ataque sea destruido.
		if (collider.gameObject.tag == "missile") {
			DeleteMissile ();
		}
	}
}
