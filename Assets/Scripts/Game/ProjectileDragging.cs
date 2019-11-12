using UnityEngine;
using UnityEngine.EventSystems;

public class ProjectileDragging : MonoBehaviour {
	public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;  
	public Transform TargetPoint;
	public float maxY;
	public float minY;
	public float maxX;
	public float minX;

	public AudioClip startSound;

	public AudioClip RunSound;
	
	private SpringJoint2D spring;
	private Transform catapult;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float boxSize, by, bx;
	private bool clickedOn;
	private Vector2 prevVelocity;

	public Transform target;
	private Vector3 v_diff;
	private float atan2;

	private Rigidbody2D rb;
	
	
	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}
	
	void Start () {
		LineRendererSetup ();
		rayToMouse = new Ray(catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		BoxCollider2D box = GetComponent<BoxCollider2D>();
		by = box.size.y;
		bx = box.size.x;
		boxSize = bx - by;

		rb = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {

	
			/* #if UNITY_ANDROID
		 if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				spring.enabled = false;
				clickedOn = true;
			}

			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				spring.enabled = true;
				GetComponent<Rigidbody2D> ().isKinematic = false;
				clickedOn = false;
			Input.touchCount = 0;
			}
		} 
		#endif */
			if (clickedOn)
				Dragging ();
		
		if (spring != null) {
				if (!rb.isKinematic && prevVelocity.sqrMagnitude > rb.velocity.sqrMagnitude) {
					Destroy (spring);
					rb.velocity = prevVelocity;
				}
			
				if (!clickedOn)
					prevVelocity = rb.velocity;
			
				LineRendererUpdate ();
	
			} else {
				if (!GameManager.GameM.RunGame && GameManager.GameM.StartGame) {
				
					catapultLineFront.enabled = false;
					catapultLineBack.enabled = false;
					//Iniciar juego
					GameManager.GameM.StartGame = false;
					GameManager.GameM.RunGame = true;
					BunnyManager.BunnyM.StartCoroutine ("ActivateBoxEffect");
					Debug.Log ("Solto");

					SoundManager.SoundM.StartAudio (startSound, 1f);
					SoundManager.SoundM.StartBackAudio (RunSound, 0.6f);

				}
			}

	}
	
	void LineRendererSetup () {
		catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);
		
		//catapultLineFront.sortingLayerName = "Foreground";
		//catapultLineBack.sortingLayerName = "Foreground";
		
		//catapultLineFront.sortingOrder = 3;
		//catapultLineBack.sortingOrder = 1;
	}
	
void OnMouseDown () {
		if (!GameManager.GameM.RunGame || GameManager.GameM.EndGame) {
			if (!EventSystem.current.IsPointerOverGameObject() && GameManager.GameM.StartGame) {
				spring.enabled = false;
				clickedOn = true;
			}
		}
}
	
void OnMouseUp () {
		if (!GameManager.GameM.RunGame || GameManager.GameM.EndGame) {
			if (!EventSystem.current.IsPointerOverGameObject () && GameManager.GameM.StartGame) {
				spring.enabled = true;
				rb.isKinematic = false;
				clickedOn = false;
			}
		}
}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
		}
		mouseWorldPoint.x = Mathf.Clamp (mouseWorldPoint.x, minX, maxX);
		mouseWorldPoint.z = 0;
		mouseWorldPoint.y = Mathf.Clamp (mouseWorldPoint.y, minY, maxY);
		transform.position = mouseWorldPoint;

		v_diff = (target.position - transform.position);    
		atan2 = Mathf.Atan2 ( v_diff.y, v_diff.x );
		transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg );
	}

	void LineRendererUpdate () {
		
		Vector3 projectilepos = new Vector2 (TargetPoint.transform.position.x, TargetPoint.transform.position.y);
		Vector2 catapultToProjectile = projectilepos - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + (boxSize + by - bx));
		catapultLineFront.SetPosition(1, holdPoint);
		catapultLineBack.SetPosition(1, holdPoint);
	}
}
