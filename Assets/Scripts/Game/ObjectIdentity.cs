using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIdentity : MonoBehaviour {
	public int Object_ID;
	public AudioClip SoundObject;
	void OnTriggerEnter2D(Collider2D coll){

		//Colisiones para los objetos
		if (coll.gameObject.tag == "bunny" || coll.gameObject.tag == "shield") {
			SoundManager.SoundM.StartAudio (SoundObject, 1f);
			if (Object_ID == 1) {
				GameManager.GameM.CalculateCarrotsGame (0);
			} else if (Object_ID == 2) {
				GameManager.GameM.CalculateFuel (2);
			}
			Destroy (gameObject);
		}
	}
}
