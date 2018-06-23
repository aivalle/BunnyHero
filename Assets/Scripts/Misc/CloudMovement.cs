using UnityEngine;
using System.Collections;

public class CloudMovement : MonoBehaviour {


	public float forwardMovementSpeed = -2;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () 
	{
		Vector2 newVelocity = rb.velocity;
		newVelocity.x = forwardMovementSpeed;
		rb.velocity = newVelocity;
	}


}
