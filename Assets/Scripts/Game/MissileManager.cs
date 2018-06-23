using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MissileManager : MonoBehaviour {

	public GameObject target;
	public GameObject explosion;
	public ParticleSystem particles;

	public AttackManager attackPrefab;

	public float speed = 5f;
	public float rotateSpeed = 200f;
	public bool life;

	private IEnumerator coroutine;


	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		//Obtiene posición del hermano en index 0
		target = GameObject.FindGameObjectWithTag("bunny");

		life = true;
	}

	void FixedUpdate () {
		if (!target || GameManager.GameM.RunGame == false) {
			Destroy (gameObject);
		} else {
			if (life == true && GameManager.GameM.GamePause == false) {
				particles.Play();
				rb.simulated = true;
				rb.isKinematic = false;
				//rb.constraints = RigidbodyConstraints2D.None;
				Vector2 direction = (Vector2)target.transform.position - rb.position;

				direction.Normalize ();

				float rotateAmount = Vector3.Cross (direction, transform.up).z;

				rb.angularVelocity = -rotateAmount * rotateSpeed;

				rb.velocity = transform.up * speed;
			} else {

				//rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
				rb.isKinematic = true;
				rb.simulated = false;
				particles.Pause();

			}
		}
	}

	void DeleteMissile(){
		if (life == true) {
			life = false;
			ParticleSystem emit = gameObject.transform.GetChild (2).GetComponent<ParticleSystem> ();
			emit.transform.parent = null;
			emit.emissionRate = 0;
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}

	void OnMouseDown()
	{
		AttackManager attack = (AttackManager)Instantiate(attackPrefab, target.transform.position,  Quaternion.Inverse(this.transform.rotation));
		attack.target = this.gameObject.gameObject;
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (life == true) {
			if (collider.gameObject.tag == "bunny") {
				BunnyManager.BunnyM.StartCoroutine ("Blinker");
				Debug.Log ("Impacto");
				DeleteMissile ();
				GameManager.GameM.SetHits (-1);
				GameManager.GameM.AnimUIUser.Play ("HitUI");

			} else if (collider.gameObject.tag == "floor" || collider.gameObject.tag == "attack" || collider.gameObject.tag == "shield") {
				DeleteMissile ();
			}
		}
	}


		
}
