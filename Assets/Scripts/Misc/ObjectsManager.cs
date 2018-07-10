using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ObjectsManager : MonoBehaviour {


	public static ObjectsManager ObjectsM;

	public List<ObjectID> objects = new List<ObjectID>();

	public List<ObjectID> vehicles = new List<ObjectID>();

	public List<Sprite> images_objects; 

	void Awake () {
		ObjectsM = this;
	}

	// Use this for initialization
	void Start () {
		objects.Add( new ObjectID(1,10,0,"object_1","desc_object_1",1,-1));
		objects.Add( new ObjectID(2,20,1,"object_2","desc_object_2",1,-1));
		objects.Add( new ObjectID(3,15,2,"object_3","desc_object_3",1,-1));

	}

	public string GetDescObject(int ID){
		return objects[ID-1].desc_text;
	}

	public string GetNameObject(int ID){
		return objects[ID-1].name_text;
	}

	public Sprite GetImageObject(int ID){
		return images_objects [objects[ID-1].image_index];
	}


	// Sistema que permite el uso de los objetos en el juego.
	public void UseObject(int ID_Object, Button button_gO = null){
		if (button_gO != null) {
			button_gO.interactable = false;
		}
		GameManager.GameM.CurrentObject.Add(ID_Object);
		if (ID_Object == 1) { //Shield bunny
			StartCoroutine (activeShieldBunny (button_gO));
		} else if (ID_Object == 2) {
			StartCoroutine (activeMagnet (button_gO));
		} else if (ID_Object == 3) {
			StartCoroutine (activeExtraVelocity(button_gO));
		}

	}

	//OBJETO: Shield
	public IEnumerator activeShieldBunny(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		GameManager.GameM.CircleColl2D.enabled = false;
		BunnyManager.BunnyM.shieldBunny.SetActive (true);
		float newtime = 5;
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		GameManager.GameM.CircleColl2D.enabled = true;
		BunnyManager.BunnyM.shieldBunny.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(1);
		if (button != null) {
			button.interactable = true;
		}
	}

	//OBJETO: Extra velocity
	public IEnumerator activeExtraVelocity(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		int adds = 0;
		float newtime = 10;
		while (adds < 3) {
			BunnyManager.BunnyM.rb.AddForce (new Vector2 (1500, 280));
			adds++;
		}
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(1);
		if (button != null) {
			button.interactable = true;
		}
	}

	//OBJETO: Magnet
	public IEnumerator activeMagnet(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		BunnyManager.BunnyM.MagnetEffect.SetActive (true);
		float newtime = 20;
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		BunnyManager.BunnyM.MagnetEffect.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(2);
		if (button != null) {
			button.interactable = true;
		}
	}

	public IEnumerator GenericObject(float duration, int Object_ID, Button button = null){
		float newtime = 0;
		while (newtime < duration) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime++;
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		GameManager.GameM.CurrentObject.Remove (Object_ID);
		if (button != null) {
			button.interactable = true;
		}
	}


}
