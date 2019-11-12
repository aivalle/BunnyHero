using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MissileManager : MonoBehaviour {

    public enum MissileT
    {
        Follow,
        Linear,
        DownUp
    }
    public MissileT MissileType;
    public GameObject target;
	public GameObject explosion;
    public GameObject childMissile;
	public ParticleSystem particles;

	public AttackManager attackPrefab;

	public float speed = 5f;
	public float rotateSpeed = 200f;
	public bool life;
    private bool waiting;

	private IEnumerator coroutine;
    private GameObject actualMissile;


	private Rigidbody2D rb;
    private Rigidbody2D rbChild;
    private bool targetInRadius;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        //Obtiene posición del hermano en index 0
        target = GameObject.FindGameObjectWithTag("bunny");
        life = true;
        waiting = true;
        targetInRadius = false;
        if (MissileType == MissileT.DownUp && childMissile)
        {
            rbChild = childMissile.GetComponent<Rigidbody2D>();
            rbChild.bodyType = RigidbodyType2D.Static;
        }
        else
        {
            MissileType = MissileT.Linear;
        }
        transform.rotation = Quaternion.Euler(0, 0, Quaternion.LookRotation(target.transform.position).z);
    }

	void FixedUpdate () {
		if (!target || !GameManager.GameM.RunGame) {
			Destroy (gameObject);
		} else {
			if (life && !GameManager.GameM.GamePause) {
				particles.Play();

                if (MissileType == MissileT.Follow)
                {
                    rb.simulated = true;
                    rb.isKinematic = false;
                    Vector2 direction = (Vector2)target.transform.position - rb.position;
                    direction.Normalize();
                    float rotateAmount = Vector3.Cross(direction, transform.up).z;
                    rb.angularVelocity = -rotateAmount * rotateSpeed;
                    rb.velocity = transform.up * speed;
                }
                else if (MissileType == MissileT.Linear)
                {
                    rb.simulated = true;
                    rb.drag = 0;
                    rb.velocity = transform.right * -speed;
                    float step = speed * Time.deltaTime;
                    Vector2 direction = new Vector2(transform.position.x, Mathf.Lerp(transform.position.y, target.transform.position.y, 0.7f * Time.deltaTime));
                    transform.position = direction;
                }
                else if (MissileType == MissileT.DownUp)
                {
                    rb.simulated = true;
                    rb.drag = 0;
                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    if (distance < 25f && waiting) {
                        waiting = false;
                        childMissile.transform.rotation = Quaternion.Euler(0, 0, Quaternion.LookRotation(transform.position).z);
                        rbChild.bodyType = RigidbodyType2D.Dynamic;
                    }
                    if (!waiting) {
                        rbChild.velocity = transform.up * speed;
                        if (distance < 5f)
                            targetInRadius = true;
                        else
                            targetInRadius = false;
                    }
                }

            } else {
              
                //rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX;
                rb.isKinematic = true;
				rb.simulated = false;
				particles.Pause();

			}
		}
	}

	void DeleteMissile(){
		if (life) {
			life = false;
			ParticleSystem emit = particles;
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
		if (life) {
			if (collider.gameObject.tag == "bunny" && MissileType != MissileT.DownUp) {
                DamageBunny();


            } else if (collider.gameObject.tag == "floor" || collider.gameObject.tag == "attack" || collider.gameObject.tag == "shield") {
                if (targetInRadius)
                    DamageBunny();

                DeleteMissile ();
			}
		}
	}

    void DamageBunny() {
        BunnyManager.BunnyM.StartCoroutine("Blinker");
        Debug.Log("Impacto");
        DeleteMissile();
        GameManager.GameM.SetHits(-1);
        GameManager.GameM.AnimUIUser.Play("HitUI");
    }


		
}
