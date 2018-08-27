using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class FacebookScript : MonoBehaviour {
	public GameObject btnL;
	public Image ProfilePic;

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
		if (FB.IsLoggedIn) {
			FB.API ("me/picture?type=square&height=80&width=80", HttpMethod.GET, GetPicture);
			btnL.SetActive (false);
		} else {
			btnL.SetActive (true);
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
		if (FB.IsLoggedIn) {
			FB.API ("me/picture?type=square&height=80&width=80", HttpMethod.GET, GetPicture);
			btnL.SetActive (false);
		} else {
			btnL.SetActive (true);
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	public void LoginFb(){
		var perms = new List<string>(){"public_profile"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}
		
	private void AuthCallback (ILoginResult result) {
		if (result.Error != null) {
			Debug.Log (result.Error);
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//		if (FB.IsLoggedIn) {
//			FB.API ("me/picture?type=square&height=80&width=80", HttpMethod.GET, GetPicture);
//			btnL.SetActive (false);
//		} else {
//			btnL.SetActive (true);
//		}
//			}
//		} else {
//			Debug.Log("User cancelled login");
//		}
	}

	private void GetPicture(IGraphResult result){
		if (result.Error == null && result.Texture != null) {
//			ProfilePic.enabled = true;
			ProfilePic.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 80, 80), new Vector2 ());
		}
	}
}
