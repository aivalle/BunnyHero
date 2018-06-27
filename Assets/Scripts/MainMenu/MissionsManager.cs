using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class MissionsManager : MonoBehaviour {

	public static MissionsManager MissionsM;

	public GameObject MissionsMenu;
	public GameObject missionsPrefab;
	public GameObject MissionsList;
	public GameObject DecorationUI;

	public Button InfiniteButton;
	public Button GameModeButton;
	public int worldID;
	public int LastMissionCompleted;

	public ScrollRectSnap ScrollMissions;

	public bool MissionsCreated;
	List<MissionID> mission;
	List<MissionID> missionW1 = new List<MissionID>();
	List<MissionID> missionW2 = new List<MissionID>();

	void CreateMissions(){

		//Add missions            ID, worldID, "desc",time,hits,distance, rewardexp,  rewards ({id,quantity}), hitmode, objectsAvailable, force final rewards
		//WORLD 1
		missionW1.Add( new MissionID(1,1,"mission_desc",60,3,200,100,new Dictionary <int,int>(){{1,2}},1,0,0));
		missionW1.Add( new MissionID(2,1,"mission_desc",-1,3,255,125,new Dictionary <int,int>(){{2,1}},2,0,0));
		missionW1.Add( new MissionID(3,1,"mission_desc",-1,3,260,150,new Dictionary <int,int>(){{1,1},{2,1}},1,1,0));
		missionW1.Add( new MissionID(4,1,"mission_desc",50,3,265,150,new Dictionary <int,int>(){{1,1},{2,2},{3,1}},2,1,0));
		missionW1.Add( new MissionID(5,1,"mission_desc",-1,3,300,175,new Dictionary <int,int>(){{1,1},{2,1},{3,2}},2,0,0));
		missionW1.Sort();

		//WORLD 2
		missionW2.Add( new MissionID(1,2,"mission_desc2",60,3,250,100,new Dictionary <int,int>(){{1,1}},1,0,0));
		missionW2.Add( new MissionID(2,2,"mission_desc2",-1,3,255,125,new Dictionary <int,int>(){{1,1},{3,2}},2,0,0));
		missionW2.Add( new MissionID(3,2,"mission_desc2",-1,3,260,150,new Dictionary <int,int>(){{2,1}},1,1,0));
		missionW2.Add( new MissionID(4,2,"mission_desc2",50,3,265,150,new Dictionary <int,int>(){{2,1}},2,1,0));
		missionW2.Add( new MissionID(5,2,"mission_desc2",-1,3,300,175,new Dictionary <int,int>(){{1,1},{2,1}},2,0,0));
		missionW2.Sort();

	}


	void Awake () {
		MissionsM = this;
	}
	void Start(){
		//Missions_menu_start ();
		CreateMissions ();
		CheckForGameModes ();
	}

	void CheckForGameModes(){
       //Infite mode
		if (UserInfo.UserI.missionsComplete.ContainsKey (1.ToString())) {
			InfiniteButton.interactable = true;
			InfiniteButton.transform.GetChild (0).gameObject.SetActive (false);
		}
		//other mode
		if (UserInfo.UserI.missionsComplete.ContainsKey (1.ToString())) {
			GameModeButton.interactable = true;
			GameModeButton.transform.GetChild (0).gameObject.SetActive (false);
		}
	}
		

	public void LoadMissionsData(int WorldID)

	{
		if (WorldID == 1) {
			mission = missionW1;
		} else {
			mission = missionW2;
		}
		worldID = WorldID;
		if (MissionsCreated == false) {

			ScrollMissions.work = false;
			Array.Clear (ScrollMissions.bttn, 0, ScrollMissions.bttn.Length);
			for(int i = 0; i < MissionsList.transform.childCount; i++)
				Destroy(MissionsList.transform.GetChild(i).gameObject);

			Array.Resize (ref ScrollMissions.bttn, mission.Count);
			LastMissionCompleted = 0;
			foreach (MissionID missions in mission) {
				GameObject missionPrefab = (GameObject)Instantiate (missionsPrefab, transform.position, Quaternion.identity);
				missionPrefab.transform.SetParent (MissionsList.transform,false);
				// Asignar el prefab de UI de la misión a la lista de Scroll.
				ScrollMissions.bttn [missions.ID - 1] = missionPrefab;
				//Crear gameobject
				missionPrefab.name = missions.ID.ToString ();

				missionPrefab.transform.GetChild (2).GetComponent<MissionButton> ().missionID = missions.ID;
				missionPrefab.transform.GetChild (1).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = missions.ID.ToString ();
				missionPrefab.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
				ScrollSprite ImgReward = missionPrefab.transform.GetChild (0).transform.GetChild (1).GetComponent<ScrollSprite> ();
				bool isCompleted = false;
				if (UserInfo.UserI.missionsComplete.ContainsKey(WorldID.ToString())) {

					if (UserInfo.UserI.missionsComplete [WorldID.ToString ()].Contains (missions.ID))
						isCompleted = true;
					else
						isCompleted = false;
				} else {
					isCompleted = false;
				}

				if (isCompleted) {
					Debug.Log ("Mision:" + missions.ID + "- COMPLETADA -");
					LastMissionCompleted = missions.ID;
					missionPrefab.transform.GetChild (2).GetComponent<Button> ().interactable = false;
					missionPrefab.transform.GetChild (2).transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = "Completada";
					ImgReward.isChecked = true;
				} else {
					if (LastMissionCompleted + 1 != missions.ID) {
						missionPrefab.transform.GetChild (2).GetComponent<Button> ().interactable = false;
						missionPrefab.transform.GetChild (2).transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = "Bloqueada";
						ImgReward.isChecked = false;
					}
					Debug.Log ("Mision:" + missions.ID + "- NO COMPLETADA -");

					if (missions.ID < LastMissionCompleted + 3) {
						List<Sprite> listSprites = new List<Sprite> ();
						foreach (var rewardID in missions.rewards) {
							listSprites.Add (ObjectsManager.ObjectsM.GetImageObject (rewardID.Key));
						}
						ImgReward.listSprites = listSprites;
					}

				}
			}

			for(int i = 0; i < DecorationUI.transform.childCount; i++)
				Destroy(DecorationUI.transform.GetChild(i).gameObject);

			GameObject UIMenuPrefab = (GameObject)Instantiate (WorldManager.WorldM.worlds[WorldID-1].UIMenu, transform.position, Quaternion.identity);
			UIMenuPrefab.transform.SetParent (DecorationUI.transform,false);
			UIMenuPrefab.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);

			//Iniciar el sistema de ScrollRect
			ScrollMissions.WorkNow ();
			//Iniciar la UI desde el prefab de la última misión
			ScrollMissions.Gotobutton (LastMissionCompleted);

			MissionsCreated = true;
		}
	}
		

	//Función para cuando le de al botón de iniciar misión.
	public void SelectMission(int mission_ID, List<MissionID> alternaMission = null){

		bool reduct = RewardSystem.RewardS.CalculateFuelGame (-1);
		if (reduct == true) {
			List<MissionID> missionI;
			int missionarray = 0;
			if (mission_ID == 0) {
				Debug.Log ("Iniciando modo de juego.");
				missionI = alternaMission;
				missionarray = 0;
			} else {
				missionI = mission;
				missionarray = mission_ID - 1;
				Debug.Log ("Iniciando mision: " + mission_ID.ToString ());
			}
				
			MissionInfo.MissionI.ActualMission = mission_ID;
			MissionInfo.MissionI.desc = missionI [missionarray].desc;
			MissionInfo.MissionI.worldID = missionI [missionarray].worldID;
			MissionInfo.MissionI.max_tiempo = missionI [missionarray].time;
			MissionInfo.MissionI.max_distance = missionI [missionarray].distance;
			MissionInfo.MissionI.max_golpes = missionI [missionarray].hits;
			MissionInfo.MissionI.maxDistance_missil = 140;
			MissionInfo.MissionI.minDistance_missil = 120;
			MissionInfo.MissionI.RewardEXP =  missionI [missionarray].rewardEXP;
			MissionInfo.MissionI.rewards = missionI [missionarray].rewards;
			MissionInfo.MissionI.hitMode = missionI [missionarray].hitMode;
			MissionInfo.MissionI.objectsAvaliable = missionI [missionarray].objectsAvailable;
			MissionInfo.MissionI.final_reward = missionI [missionarray].final_reward;
			MissionInfo.MissionI.ActualAssets = WorldManager.WorldM.worlds [missionI [missionarray].worldID - 1].GameAssets;
			LevelManager.LevelM.LoadScene ("Game");
		} 
	}

	public void MissionComplete(int mission_ID, int worldID){
		if (mission_ID == 0) {
			//If mission is infinite or other game mode
		} else {
			List<int> newM = new  List<int>();
			if (UserInfo.UserI.missionsComplete.ContainsKey (worldID.ToString ())) {
				newM = UserInfo.UserI.missionsComplete [worldID.ToString ()];
				UserInfo.UserI.missionsComplete [worldID.ToString ()].Add (mission_ID);
			} else {
				newM.Add (mission_ID);
				UserInfo.UserI.missionsComplete.Add (worldID.ToString(), newM);
			}

		}
	}

	public void MissionsClear(){
		UserInfo.UserI.missionsComplete.Clear();
	}

	public void SaveAllData(){
		Serializer.serializer.SaveInfo ();
	}
}