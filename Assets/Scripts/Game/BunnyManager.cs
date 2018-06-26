using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyManager : MonoBehaviour {

	//Este script es aplicado a la entidad del conejo principal. 

	public static BunnyManager BunnyM;
	public float YForceUp;
	public float XForce;
	private float angle;
	public GameObject dust;
	public bool grounded;

	public Rigidbody2D rb;
	private ParticleSystem.EmissionModule em;

	void Awake(){
		BunnyM = this;
	}

	// Use this for initialization
	void Start () {
		em = dust.GetComponent <ParticleSystem> ().emission;
		em.enabled = false;
		rb = GetComponent<Rigidbody2D> ();
		em = dust.GetComponent <ParticleSystem> ().emission;
	}


	void FixedUpdate ()
	{
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


		if (GameManager.GameM.RunGame == true && GameManager.GameM.GamePause == false || GameManager.GameM.StartGame == true) {
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
		} 
	}

	void OnCollisionExit2D(Collision2D coll) { // Si el conejo sale de tocar el suelo entonces desactiva las particulas y una variable 
		if (coll.gameObject.tag == "floor") {
			grounded = false;
			em.enabled = false;
		}
	}

	//Efecto de daño aplicado a la imagen del conejo.
	public IEnumerator Blinker(){
		SpriteRenderer sprite = gameObject.transform.GetChild (0).GetComponent<SpriteRenderer> ();
		sprite.color = new Color32 (255,186,186,127);
		yield return new WaitForSeconds (0.2f);
		sprite.color =  new Color32 (255,255,255,255);
		yield return new WaitForSeconds (0.1f);
		sprite.color = new Color32 (255,186,186,127);
		yield return new WaitForSeconds (0.2f);
		sprite.color = new Color32 (255,255,255,255);
		StopCoroutine ("Blinker");
	}

}
