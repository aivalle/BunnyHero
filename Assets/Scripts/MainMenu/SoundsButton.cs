using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsButton : MonoBehaviour {

	public static SoundsButton SoundB;

	public AudioClip SoundButton1;
	public AudioClip SoundButton2;

	// Use this for initialization
	void Awake () {
		SoundB = this;
	}
	
	// Update is called once per frame
	public void SoundButton (int type) {
		if (type == 1) {
			SoundManager.SoundM.AudioS.PlayOneShot (SoundButton1);
		} else if (type == 2) {
			SoundManager.SoundM.AudioS.PlayOneShot (SoundButton2);
		}
	}
}
