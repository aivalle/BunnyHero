using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Advertisement.Initialize("2822114");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RequestVideo(){
		ShowOptions so = new ShowOptions ();
		so.resultCallback = getReward;
		Advertisement.Show ("rewardedVideo",so);
	}

	private void getReward(ShowResult sr){
		if (sr == ShowResult.Finished) {
			Debug.Log ("Ad finish successfully");
		} else {
			Debug.Log ("Ad not completed");
		}
	}
}
