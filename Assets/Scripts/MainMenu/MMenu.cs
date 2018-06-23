using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMenu : MonoBehaviour {

	public static MMenu MainM;

	public GameObject WindowMissions;
	public GameObject WindowWorldMissions;
	public GameObject WindowGameMode;
	public GameObject WindowSettings;
	public GameObject WindowShop;
	public GameObject WindowPButtons;
	public GameObject WindowDecoMission;

	public GameObject ActualWindow;
	public AudioClip MenuSound;

	public bool ViewChanged;
	void Awake () {
		MainM = this;
	}
	// Use this for initialization
	void Start () {
		WindowMissions.SetActive (false);
		WindowGameMode.SetActive (false);
		WindowSettings.SetActive (false);
		WindowShop.SetActive (false);
		WindowWorldMissions.SetActive (false);
		WindowPButtons.SetActive (true);
		ViewChanged = false;

		SoundManager.SoundM.StartBackAudio (MenuSound, 0.6f);
	}
	
	// Update is called once per frame
	void ChangeViewCamera (bool opc) {
		if (opc == true) {
			Animation anim  = WindowDecoMission.GetComponent<Animation> ();
			Animation camera = Camera.main.GetComponent<Animation> ();

			anim ["decorationUIM"].speed = 1;
			anim ["decorationUIM"].time = 0.0f;
			anim.Play ("decorationUIM");

			camera ["CameraMoveDown"].speed = 1;
			camera ["CameraMoveDown"].time = 0.0f;
			camera.Play ("CameraMoveDown");

			ViewChanged = true;

			WindowPButtons.SetActive (false);

		} else {
			Animation anim  = WindowDecoMission.GetComponent<Animation> ();
			Animation camera = Camera.main.GetComponent<Animation> ();

			anim ["decorationUIM"].speed = -1;
			anim ["decorationUIM"].time = anim ["decorationUIM"].length;
			anim.Play ("decorationUIM");

			camera ["CameraMoveDown"].speed = -1;
			camera ["CameraMoveDown"].time = camera ["CameraMoveDown"].length;
			camera.Play ("CameraMoveDown");

			ViewChanged = false;
		}
	}

	public void Buttom_Do(string show_menu)
	{
		Animation anim;
		Debug.Log ("Abriendo menú... " + show_menu);
		if (ActualWindow) {
			anim = ActualWindow.GetComponent<Animation> ();
		
				anim ["WindowMissions"].speed = -1;
				anim ["WindowMissions"].time = anim ["WindowMissions"].length;
				anim.Play ("WindowMissions");

		}
		if (PlayerLevelSystem.PLevelS.UIEXPEnable == true)
			PlayerLevelSystem.PLevelS.ActivateUIEXP (false);

		if (ViewChanged == true) {
			ChangeViewCamera (false);
		}

		switch (show_menu)
		{
		case "mode_games":
			
			WindowGameMode.SetActive (true);
			anim = WindowGameMode.GetComponent<Animation> ();
			anim ["WindowMissions"].speed = 1;
			anim ["WindowMissions"].time = 0.0f;
			anim.Play ("WindowMissions");


			ActualWindow = WindowGameMode;
			break;

		case "world_select":
			WindowWorldMissions.SetActive (true);
			anim =  WindowWorldMissions.GetComponent<Animation> ();
			anim ["WindowMissions"].speed = 1;
			anim ["WindowMissions"].time = 0.0f;
			anim.Play ("WindowMissions");
			ActualWindow = WindowWorldMissions;
			WindowPButtons.SetActive (false);
			break;


		case "missions":
			WindowMissions.SetActive (true);
			anim = WindowMissions.GetComponent<Animation> ();
			anim ["WindowMissions"].speed = 1;
			anim ["WindowMissions"].time = 0.0f;
			anim.Play ("WindowMissions");

			ChangeViewCamera (true);

			MissionsManager.MissionsM.ScrollMissions.Gotobutton (MissionsManager.MissionsM.LastMissionCompleted);
			ActualWindow = WindowMissions;
			break;


		case "close_all":
			if (PlayerLevelSystem.PLevelS.UIEXPEnable == false)
				PlayerLevelSystem.PLevelS.ActivateUIEXP (true);
			ActualWindow = null;
			WindowPButtons.SetActive (true);
			break;

		case "settings":
			WindowSettings.SetActive (true);
			anim = WindowSettings.GetComponent<Animation> ();
			anim ["WindowMissions"].speed = 1;
			anim ["WindowMissions"].time = 0.0f;
			anim.Play ("WindowMissions");

			ActualWindow = WindowSettings;
			break;

		case "shop":
			WindowShop.SetActive (true);
			anim = WindowShop.GetComponent<Animation> ();
			anim ["WindowMissions"].speed = 1;
			anim ["WindowMissions"].time = 0.0f;
			anim.Play ("WindowMissions");

			ActualWindow = WindowShop;
			break;


		default:
			Debug.Log("No se encuentra el menu " + show_menu);
			break;
		}
			
	}
		
}
