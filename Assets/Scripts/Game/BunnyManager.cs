using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MonoBehaviour {

	//Este script es aplicado a la entidad del conejo principal. 

	public static BunnyManager BunnyM;
	public float YForceUp;
	public float XForce;

	public float ActualHeight;
	private float angle;
	public GameObject dust;
	public GameObject parachute;
	public GameObject box;
	public GameObject shieldBunny;
	public GameObject MagnetEffect;

	public GameObject ExplosionEffect;

	public ParticleSystem BoxEffect;
	public bool grounded;
	public bool Click;

	public Rigidbody2D rb;
	private ParticleSystem.EmissionModule em;

	void Awake(){
		BunnyM = this;
	}

	// Use this for initialization
	void Start () {
		em = dust.GetComponent <ParticleSystem> ().emission;
		em.enabled = false;

		var emBox = BoxEffect.emission;
		emBox.enabled = false;
		rb = GetComponent<Rigidbody2D> ();
	}


	void FixedUpdate ()
	{
		CalculateHeight ();
		//Si el juego no está en pausa entonces hay simulación con el enterno (gravedad).
		if (GameManager.GameM.GamePause == false) {
			rb.simulated = true;
		} else {
			rb.simulated = false;
		}


		// Sistema que permite rotar al conejo dependiendo su direción y velocidad.
		Vector2 v = rb.velocity;
		angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));


		if (Click == true) {
			dust.transform.position = new Vector2 (transform.position.x, dust.transform.position.y);

			//Si el usuario esta presionando la pantalla:
			bool clickActive = Input.GetButton ("Fire1");
			if (clickActive) {
				
				if (GameManager.GameM.fuel > 0) { //Si tiene gasolina le permite elevarse y obtener velocidad.
					rb.AddForce (new Vector2 (XForce, YForceUp));

					if (GameManager.GameM.RunGame == true && GameManager.GameM.GamePause == false) {// Descuenta la gasolina aplicada con el click si el juego esta sin pausa
						GameManager.GameM.CalculateFuel (1);
					}
				}
	
			}
		}
			
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") { // Si el conejo está tocando el suelo entonces activa las particulas y una variable 
			grounded = true;	
			em.enabled = true;
			parachute.SetActive (false);
		} 
	}

	void OnCollisionExit2D(Collision2D coll) { // Si el conejo sale de tocar el suelo entonces desactiva las particulas y una variable 
		if (coll.gameObject.tag == "floor") {
			dust.transform.position = new Vector2 (transform.position.x, dust.transform.position.y);
			grounded = false;
			em.enabled = false;
		}
	}

	public void CalculateHeight(){
		ActualHeight = this.gameObject.gameObject.transform.position.y - dust.transform.position.y;
	}

	//Efecto de daño aplicado a la imagen del conejo.
	public IEnumerator Blinker(){
		SpriteRenderer sprite = gameObject.transform.GetChild (0).GetComponent<SpriteRenderer> ();
		sprite.color = new Color32 (255,186,186,255);
		yield return new WaitForSeconds (0.2f);
		sprite.color =  new Color32 (255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		sprite.color = new Color32 (255,186,186,255);
		yield return new WaitForSeconds (0.2f);
		sprite.color = new Color32 (255,255,255,255);
		StopCoroutine ("Blinker");
	}

	public IEnumerator ActivateBoxEffect(){
		yield return new WaitForSeconds (0.5f);
		BunnyManager.BunnyM.Click = true;
		ActivateParticles (true);
	}

	public IEnumerator EjectBox(){
		parachute.SetActive (true);
		parachute.GetComponent <Animation> ().Play ("Parachute");

		GameObject boxPrefab = (GameObject)Instantiate (box, transform.position, Quaternion.identity);

		box.SetActive (false);
		Rigidbody2D rbox = boxPrefab.GetComponent <Rigidbody2D> ();
		rbox.velocity = rb.velocity;
		rbox.isKinematic = false;
		rb.velocity = new Vector2 (12, 15);
		rb.gravityScale = 0.6f;

		boxPrefab.GetComponent <BoxCollider2D> ().enabled = true;
		yield return new WaitForSeconds (0.1f);
	}

	public void ActivateParticles(bool opc){
		var em = BoxEffect.emission;
		if (opc) {
			em.enabled = true;
		} else {
			em.enabled = false;
		}
	}


}
