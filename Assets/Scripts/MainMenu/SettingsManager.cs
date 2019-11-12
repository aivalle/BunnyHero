using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

	public static SettingsManager SettingsM;
	public bool AmbientMusic = true;
	public bool SoundEffects = true;


	// Use this for initialization
	void Awake () {
		SettingsM = this;
		if (PlayerPrefs.HasKey ("AmbientMusic")) {
			int AM = PlayerPrefs.GetInt ("AmbientMusic");
			if (AM == 0) {
				AmbientMusic = false;
			} else {
				AmbientMusic = true;
			}
			Debug.Log (AM);
		}

		if (PlayerPrefs.HasKey ("SoundEffects")) {
			int SE = PlayerPrefs.GetInt ("SoundEffects");
			if (SE == 0) {
				SoundEffects = false;
			} else {
				SoundEffects = true;
			}
			Debug.Log (SE);
		}
	}

	public void SaveSetting(){
		ApplyChanges ();
		PlayerPrefs.SetInt("AmbientMusic", AmbientMusic.GetHashCode());
		PlayerPrefs.SetInt("SoundEffects", SoundEffects.GetHashCode());
		Debug.LogFormat ("Ajustes guardados. {0},{1}",AmbientMusic.GetHashCode(),SoundEffects.GetHashCode());
	}

	public void ApplyChanges(){
		if (AmbientMusic) {

			SoundManager.SoundM.AudioBackS.volume = 1;
		} else {
			SoundManager.SoundM.AudioBackS.volume = 0;
		}

		if (SoundEffects) {
			SoundManager.SoundM.AudioS.volume = 1;
		} else {
			SoundManager.SoundM.AudioS.volume = 0;
		}
	}
}
