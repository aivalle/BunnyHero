using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractEffect : MonoBehaviour {

	private GameObject target;
	private Rigidbody2D rb;
	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float rotateSpeed = 200f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target) {

				Vector2 direction = (Vector2)target.transform.position - rb.position;
				direction.Normalize ();
				float rotateAmount = Vector3.Cross (direction, transform.up).z;
				rb.angularVelocity = -rotateAmount * rotateSpeed;
				rb.velocity = transform.up * speed;
		}
	}

	void OnTriggerEnter2D (Collider2D collider){
		
		if (collider.gameObject.tag == "magnetic_area" && GameManager.GameM.CurrentObject.Contains(2)) {
			target = collider.gameObject;
		}

	}
}