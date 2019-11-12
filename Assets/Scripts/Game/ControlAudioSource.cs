using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAudioSource : MonoBehaviour {

	public AudioSource AS;
	// Use this for initialization
	void Start () {
		if (!SettingsManager.SettingsM.SoundEffects) {
			AS.volume = 0;
		}

	}
}
