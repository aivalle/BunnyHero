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
	//public GameObject box;
	public GameObject shieldBunny;
	public GameObject MagnetEffect;

	public GameObject ExplosionEffect;
    public GameObject BunnyBox;

	//public ParticleSystem BoxEffect;
	public bool grounded;
	public bool Click;

	public Rigidbody2D rb;
	private ParticleSystem.EmissionModule em;

    public Vehicle vehicleInfo;

	void Awake(){
		BunnyM = this;
	}

	// Use this for initialization
	void Start () {
		em = dust.GetComponent <ParticleSystem> ().emission;
		em.enabled = false;

        CreateBunny();
        rb = GetComponent<Rigidbody2D> ();
	}

    void CreateBunny()
    {
        for (int x = 0; x < BunnyBox.transform.childCount; x++)
            Destroy(BunnyBox.transform.GetChild(x).gameObject);
        int ID;
        if (UserInfo.UserI.bunny.ContainsKey("box"))
            ID = int.Parse(UserInfo.UserI.bunny["box"].ToString());
        else
            ID = 1;

        GameObject bunny = ObjectsManager.ObjectsM.InstantiateVehicleInGame(ID, BunnyBox);
        bunny.transform.localPosition = new Vector3(0, 0, 0);
        vehicleInfo = bunny.GetComponent<Vehicle>();
        vehicleInfo.ActivateEffect(false);
    }


    void FixedUpdate ()
	{
		CalculateHeight ();
		//Si el juego no está en pausa entonces hay simulación con el enterno (gravedad) y dirección del conejo.
		if (!GameManager.GameM.GamePause) {
			rb.simulated = true;
			// Permite rotar al conejo dependiendo su direción y velocidad.
			Vector2 v = rb.velocity;
			angle = Mathf.Atan2 (v.y, v.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
		} else {
			rb.simulated = false;
		}
			
	}

	void Update(){
	
		if (Click) {
			//Si el usuario esta presionando la pantalla:
			if (Input.GetButton ("Fire1")) {

				if (GameManager.GameM.fuel > 0) { //Si tiene gasolina le permite elevarse y obtener velocidad.
					rb.AddForce (new Vector2 (XForce, YForceUp));

					if (GameManager.GameM.RunGame && !GameManager.GameM.GamePause) {// Descuenta la gasolina aplicada con el click si el juego esta sin pausa
						GameManager.GameM.CalculateFuel (1);
					}
				}

			}
		}

	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "floor") { // Si el conejo está tocando el suelo entonces activa las particulas y una variable 
			dust.transform.position = new Vector2 (transform.position.x, dust.transform.position.y);
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
		ActualHeight = gameObject.transform.position.y - dust.transform.position.y;
	}

	//Efecto de daño aplicado a la imagen del conejo.
	public IEnumerator Blinker(){
		SpriteRenderer sprite = vehicleInfo.BoxSpriteGObject.GetComponent<SpriteRenderer> ();
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
        vehicleInfo.ActivateEffect(true);
	}

	public IEnumerator EjectBox(){
		parachute.SetActive (true);
		parachute.GetComponent <Animation> ().Play ("Parachute");

		GameObject boxPrefab = (GameObject)Instantiate (vehicleInfo.gameObject, transform.position, Quaternion.identity);

        vehicleInfo.BoxSpriteGObject.SetActive (false);
        boxPrefab.GetComponent<Vehicle>().BunnyGObject.SetActive(false);
        Rigidbody2D rbox = boxPrefab.GetComponent <Rigidbody2D> ();
		rbox.velocity = rb.velocity;
		rbox.isKinematic = false;
		rb.velocity = new Vector2 (12, 15);
		rb.gravityScale = 0.6f;

		boxPrefab.GetComponent <BoxCollider2D> ().enabled = true;
		yield return new WaitForSeconds (0.1f);
	}
}
