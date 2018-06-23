using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnSceneWasLoaded : MonoBehaviour {

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
		
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		LevelManager.LevelM.BeginFade (-1);
	}
}
