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
	public GameObject WindowProfile;
	public GameObject WindowPButtons;
	public GameObject WindowDecoMission;
    public GameObject WindowMissionInfo;

    public List<GameObject> listWindows;

    public GameObject ActualWindow;
    public GameObject Logo;
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
		WindowProfile.SetActive (false);
		WindowWorldMissions.SetActive (false);
        WindowMissionInfo.SetActive(false);
        WindowPButtons.SetActive (true);

		ViewChanged = false;

		SoundManager.SoundM.StartBackAudio (MenuSound, 0.6f);
	}

	public void Buttom_Do(string show_menu)
	{
		Debug.Log ("Abriendo menú... " + show_menu);
		if (ActualWindow) {

            AnimateWindow(ActualWindow, true);

		}
		if (PlayerLevelSystem.PLevelS.UIEXPEnable == true)
			PlayerLevelSystem.PLevelS.ActivateUIEXP (false);

        if (show_menu == "back" && listWindows.Count <= 1)
        {
            this.Buttom_Do("close_all");
            WindowPButtons.SetActive(true);
        }
        else
            WindowPButtons.SetActive(false);

       switch (show_menu)
		{
		case "mode_games":
                AnimateWindow(WindowGameMode, false);
                ActualWindow = WindowGameMode;
			    break;

		case "world_select":

                AnimateWindow(WindowWorldMissions, false);
                ActualWindow = WindowWorldMissions;
			    WindowPButtons.SetActive (false);
			    break;


		case "missions":
                AnimateWindow(WindowMissions, false);
                MissionsManager.MissionsM.ChangeViewCamera (true);
			    ActualWindow = WindowMissions;
			    break;


		case "close_all":
			    if (PlayerLevelSystem.PLevelS.UIEXPEnable == false)
				    PlayerLevelSystem.PLevelS.ActivateUIEXP (true);
			    ActualWindow = null;
			    WindowPButtons.SetActive (true);
                listWindows.Clear();
                Logo.GetComponent<AnimatorSetValues>().ToggleBoolLogo(true);
                MissionsManager.MissionsM.ChangeViewCamera(false);
                break;

		case "settings":
                AnimateWindow(WindowSettings, false);
                ActualWindow = WindowSettings;
			    break;

		case "profile":
                AnimateWindow(WindowProfile, false);
                ActualWindow = WindowProfile;
			    break;

		case "shop":
                AnimateWindow(WindowShop, false);
                ActualWindow = WindowShop;
			    break;

        case "mission_info":
                AnimateWindow(WindowMissionInfo, false);
                ActualWindow = WindowMissionInfo;

                break;

        case "back":

                bool fixedWindows = false;
                if (listWindows.Count > 1) {
                    if (ActualWindow != listWindows[listWindows.Count - 1]) {
                        listWindows.Remove(ActualWindow);
                        AnimateWindow(listWindows[listWindows.Count - 1], false);
                        ActualWindow = listWindows[listWindows.Count - 1];
                        fixedWindows = true;
                    }
                    else
                    {
                        AnimateWindow(listWindows[listWindows.Count - 2], false);
                        ActualWindow = listWindows[listWindows.Count - 2];
                    }
                }
                if (!fixedWindows)
                {
                    if (listWindows.Count > 0)
                        listWindows.RemoveAt(listWindows.Count - 1);
                    else
                        listWindows.Clear();
                }

                if (show_menu != "mission_info" && !listWindows.Contains(WindowMissionInfo) && !listWindows.Contains(WindowMissions))
                {
                    MissionsManager.MissionsM.ChangeViewCamera(false);
                }
                break;

        default:
			Debug.Log("No se encuentra el menu " + show_menu);
			break;
		}
        if(!listWindows.Contains(ActualWindow) && show_menu != "back" && show_menu != "close_all")
            listWindows.Add(ActualWindow);
    }

    void AnimateWindow(GameObject window, bool close) {
        Animation anim;
        anim = window.GetComponent<Animation>();
        if (close) {
            anim["WindowMissions"].speed = -1;
            anim["WindowMissions"].time = anim["WindowMissions"].length;
        } else {
            window.transform.parent.gameObject.SetActive(true);
            window.SetActive(true);
            anim["WindowMissions"].speed = 1;
            anim["WindowMissions"].time = 0.0f;

        }
        anim.Play("WindowMissions");
    }
		
}
