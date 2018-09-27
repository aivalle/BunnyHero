using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GooglePlay : MonoBehaviour {
	public GameObject BtnAc, BtnLB;

	private void Awake(){
		PlayGamesPlatform.Activate ();
		if(PlayGamesPlatform.Instance.localUser.authenticated){
			Debug.Log ("Logged Already");
			BtnAc.SetActive (true); 
			BtnLB.SetActive (true);
		}else{
			BtnAc.SetActive (false); 
			BtnLB.SetActive (false);
			Social.localUser.Authenticate((bool success) =>
				{
					Debug.Log("Logged in");
				});
		}
	}
}
