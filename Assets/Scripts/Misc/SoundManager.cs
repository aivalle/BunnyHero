using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager SoundM;

	public AudioSource AudioSource { get; private set; }

	public AudioSource AudioS;
	public AudioSource AudioBackS;

	public AudioSource AudioBackEXP;

	void Awake(){
		SoundM = this;
	}

	public void StartAudio (AudioClip audio, float volume) {
		AudioS.PlayOneShot (audio, volume);
	}

	public void StartBackAudio (AudioClip audio, float volume) {
		AudioBackS.mute = false;
		AudioBackS.clip = audio;
		AudioBackS.Play ();
		AudioBackS.pitch = 1f;
		if (SettingsManager.SettingsM.AmbientMusic == true) {
			AudioBackS.volume = volume;
		}
	}

	public void StartAudioEXP (AudioClip audio, float volume) {
		AudioBackEXP.mute = false;
		AudioBackEXP.clip = audio;
		AudioBackEXP.Play ();
		if (SettingsManager.SettingsM.SoundEffects == true) {
			AudioBackEXP.volume = volume;
		} else {
			AudioBackEXP.volume = 0.0f;
		}
	}


	IEnumerator MusicBackDecrease()
	{
		float timeToDecrease = 0.1f;
		float i = 1f;
		while (i > 0f) {
			SoundManager.SoundM.AudioBackS.pitch -= timeToDecrease;
			i -= 0.1f;
			yield return new WaitForSeconds (0.2f);
		}
		MuteBackMusic (true);

	}

	public IEnumerator MusicBackDecreaseVolume()
	{
		float timeToDecrease = 0.1f;
		float i = 1f;
		while (i > 0f) {
			SoundManager.SoundM.AudioBackS.volume -= timeToDecrease;
			i -= 0.1f;
			yield return new WaitForSeconds (0.1f);
		}
		MuteBackMusic (true);

	}

	public void MuteBackMusic(bool opcion)
	{
		AudioBackS.mute = opcion;
	}


}
