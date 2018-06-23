using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	public Toggle ToggleSoundAmbient;
	public Toggle ToggleSoundEffects;
	bool SettingsDone;

	public void Start () {
		
		SettingsDone = false;
		ToggleSoundAmbient.isOn = SettingsManager.SettingsM.AmbientMusic;
		ToggleSoundEffects.isOn = SettingsManager.SettingsM.SoundEffects;
		SettingsDone = true;
		SettingsManager.SettingsM.ApplyChanges ();
	}

	// Update is called once per frame
	public void ChangeSoundSettings (int typeSound) {
		if (SettingsDone == true) {
			if (typeSound == 1) {

				SettingsManager.SettingsM.AmbientMusic = !SettingsManager.SettingsM.AmbientMusic;
				Debug.Log (SettingsManager.SettingsM.AmbientMusic);

			} else if (typeSound == 2) {

				SettingsManager.SettingsM.SoundEffects = !SettingsManager.SettingsM.SoundEffects;
				Debug.Log (SettingsManager.SettingsM.SoundEffects);
			}
		}
	}

	public void SaveAllSetting(){
		SettingsManager.SettingsM.SaveSetting ();
	}


}
