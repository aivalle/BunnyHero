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
    public GameObject PanelInfoMission;
    public GameObject ObjectsUI;

    public GameObject PanelUIObjects;
    public GameObject PanelObjects;
    public GameObject ObjectPrefab;
    public GameObject InfoWorldUI;

    public Button InfiniteButton;
	public Button GameModeButton;

    public ScrollRectSnap ScrollMissions;

    public ScrollSnapRect ScrollMissionsNew;

    public bool MissionsCreated;
    public bool ViewChanged;

    public List<int> objects_game = new List<int>();
    public int indexButton;
    public int idObjRemove;
    public int LastStandardMissionCIndex;


    Dictionary<int,Dictionary<string,MissionID>> standardMissions = new Dictionary<int,Dictionary<string,MissionID>>();

	Dictionary<string,MissionID> world1M = new Dictionary<string, MissionID>();
	Dictionary<string,MissionID> world2M = new Dictionary<string, MissionID>();

	Dictionary<string,MissionID> customsMissions = new Dictionary<string, MissionID>();

	void CreateMissions(){

		//Add missions            worldID, "desc",time,hits,distance, rewardexp,  rewards ({id,quantity}), hitmode, objectsAvailable, force final rewards, times receive reward
		//WORLD 1
		world1M.Add("m1_w1", new MissionID(1,"mission_desc",60,3,200,1,new Dictionary <int,int>(){{1,2}},1,0,0,1));
		world1M.Add("m2_w1", new MissionID(1,"mission_desc",-1,3,255,1,new Dictionary <int,int>(){{2,1}},2,0,0,1));
		world1M.Add("m3_w1", new MissionID(1,"mission_desc",-1,3,260,1,new Dictionary <int,int>(){{1,1},{2,1}},1,1,0,1));
		world1M.Add("m4_w1", new MissionID(1,"mission_desc",50,3,265,1,new Dictionary <int,int>(){{1,1},{2,2},{3,1}},2,1,0,1));
		world1M.Add("m5_w1", new MissionID(1,"mission_desc",-1,3,300,1,new Dictionary <int,int>(){{1,1},{2,1},{3,2}},2,0,0,1));

		standardMissions.Add (1, world1M);

		//WORLD 2
		world2M.Add("m1_w2", new MissionID(2,"mission_desc2",60,3,250,1,new Dictionary <int,int>(){{1,1}},1,0,0,1));
		world2M.Add("m2_w2", new MissionID(2,"mission_desc2",-1,3,255,1,new Dictionary <int,int>(){{1,1},{3,2}},2,0,0,1));
		world2M.Add("m3_w2", new MissionID(2,"mission_desc2",-1,3,260,1,new Dictionary <int,int>(){{2,1}},1,1,0,1));
		world2M.Add("m4_w2", new MissionID(2,"mission_desc2",50,3,265,1,new Dictionary <int,int>(){{2,1}},2,1,0,-1));
		world2M.Add("m5_w2", new MissionID(2,"mission_desc2",-1,3,300,1,new Dictionary <int,int>(){{1,1},{2,1}},2,0,0,1));

		standardMissions.Add (2, world2M);

		//CUSTOMS
		customsMissions.Add("inf_0", new MissionID (1, "%infinite_mode_desc%", -1,2, -1, 1, new Dictionary <int,int>(){}, 2, 1, 1,-1));

	}


	void Awake () {
		MissionsM = this;
	}
	void Start(){
		CreateMissions ();
		CheckForGameModes ();

        PanelUIObjects.SetActive(false);

        ViewChanged = false;

    }

	void CheckForGameModes(){
       //Infite mode
		if (UserInfo.UserI.missionsComplete.ContainsKey (1)) {
			InfiniteButton.interactable = true;
			InfiniteButton.transform.GetChild (1).gameObject.SetActive (false);
		}
		//other mode
		if (UserInfo.UserI.missionsComplete.ContainsKey (1)) {
			GameModeButton.interactable = true;
			GameModeButton.transform.GetChild (2).gameObject.SetActive (false);
		}
	}
		

	public void LoadMissionsData(int worldID)
	{
		if (!MissionsCreated) {

	
			Array.Clear (ScrollMissions.AllButtons, 0, ScrollMissions.AllButtons.Length);
			for(int i = 0; i < MissionsList.transform.childCount; i++)
				Destroy(MissionsList.transform.GetChild(i).gameObject);

			Array.Resize (ref ScrollMissions.AllButtons, standardMissions[worldID].Count);
			LastStandardMissionCIndex = -1;
			int missionCount = 0;
            int missionCompleted = 0;
            foreach (string mission in standardMissions[worldID].Keys) {
				GameObject missionPrefab = (GameObject)Instantiate (missionsPrefab, transform.position, Quaternion.identity);
				missionPrefab.transform.SetParent (MissionsList.transform,false);
				// Asignar el prefab de UI de la misión a la lista de Scroll.
				ScrollMissions.AllButtons[missionCount] = missionPrefab;
				//Crear gameobject
				missionPrefab.name = missionCount.ToString ();

				missionPrefab.transform.GetChild (0).GetComponent<MissionButton> ().missionID = mission;
				missionPrefab.transform.GetChild (0).GetComponent<MissionButton> ().worldID = worldID;
				//missionPrefab.transform.GetChild (1).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = mission;
				missionPrefab.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
				//ScrollSprite ImgReward = missionPrefab.transform.GetChild (0).transform.GetChild (1).GetComponent<ScrollSprite> ();
				bool isCompleted = false;
				if (UserInfo.UserI.missionsComplete.ContainsKey(worldID)) {

					if (UserInfo.UserI.missionsComplete [worldID].ContainsKey(mission))
						isCompleted = true;
					else
						isCompleted = false;
				} else {
					isCompleted = false;
				}

				if (isCompleted) {
                    missionCompleted++;

                    Debug.Log ("Mision:" + mission + "- COMPLETADA -");
					LastStandardMissionCIndex = missionCount;
					missionPrefab.transform.GetChild (0).GetComponent<Button> ().interactable = true;
					//ImgReward.isChecked = true;
				} else {
					if (LastStandardMissionCIndex + 1 != missionCount) {
						missionPrefab.transform.GetChild (0).GetComponent<Button> ().interactable = false;
						//ImgReward.isChecked = false;
					}
					Debug.Log ("Mision:" + mission + "- NO COMPLETADA -");

					if (missionCount < LastStandardMissionCIndex + 3) {
						//ImgReward.listSprites = listSprites;
					}

				}
                missionPrefab.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (missionCount+1).ToString();

                missionCount++;
			}
            //Iniciar el sistema de ScrollRect
            ScrollMissions.WorkNow();


            //Imprimir diseño del mundo
            ChangeUIWorldSelected(worldID);

            //Imprimir info. mundo
            InfoWorldUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = WorldManager.WorldM.worlds[worldID - 1].name;
            InfoWorldUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = WorldManager.WorldM.worlds[worldID - 1].desc;
            InfoWorldUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}/{1}", missionCompleted, standardMissions[worldID].Count);

			MissionsCreated = true;
            //Iniciar la UI desde el prefab de la última misión
            StartCoroutine("GoToLastMission");
        }
	}

    public IEnumerator GoToLastMission()
    {
        yield return new WaitForSeconds(1f);
        ScrollMissions.GoToButton(LastStandardMissionCIndex+1);
    }


    public void ChangeUIWorldSelected(int worldID)
    {
        for (int i = 0; i < DecorationUI.transform.childCount; i++)
            Destroy(DecorationUI.transform.GetChild(i).gameObject);
        GameObject UIMenuPrefab = (GameObject)Instantiate(WorldManager.WorldM.worlds[worldID - 1].UIMenu, transform.position, Quaternion.identity);
        UIMenuPrefab.transform.SetParent(DecorationUI.transform, false);
        UIMenuPrefab.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        UIMenuPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
        
    }
		

	//Función para cuando le de al botón de iniciar misión.
	public void SelectMission(string missionID, int worldID = 0){

            MissionID missionI;
			if (standardMissions.ContainsKey(worldID) && standardMissions [worldID].ContainsKey (missionID)) {
				Debug.Log ("Iniciando misión estándar: " + missionID);
				missionI = standardMissions [worldID] [missionID];
			} else if (customsMissions.ContainsKey (missionID)) {
				missionI = customsMissions [missionID];
				Debug.Log ("Iniciando misión custom: " + missionID);
			} else {
				Debug.Log ("No se encontró la info de " + missionID);
				return;
			}

            PanelInfoMission.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("%mission_title%"), "<gradient=\"Gradient_orange\">"+ Lean.Localization.LeanLocalization.GetTranslationText(missionID, missionID) + "</gradient> ");
            PanelInfoMission.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", Lean.Localization.LeanLocalization.GetTranslationText(missionI.desc, missionI.desc));
          
            ScrollSprite ImgReward = PanelInfoMission.transform.GetChild(5).transform.GetChild(1).GetComponent<ScrollSprite>();
            List<GameObject> listSprites = new List<GameObject>();
            ImgReward.ClearAllSprites();
            foreach (var rewardID in missionI.rewards)
            {
            listSprites.Add(ObjectsManager.ObjectsM.InstantiateObject(rewardID.Key, PanelInfoMission.transform.GetChild(5).transform.GetChild(1).gameObject, rewardID.Value));
            }

            bool isCompleted = false;
            if (UserInfo.UserI.missionsComplete.ContainsKey(worldID))
            {

                if (UserInfo.UserI.missionsComplete[worldID].ContainsKey(missionID))
                    isCompleted = true;
                else
                    isCompleted = false;
            }
            else
            {
                isCompleted = false;
            }

        if (isCompleted)
            ImgReward.isChecked = true;
        else
            ImgReward.isChecked = false;

        ImgReward.listSprites = listSprites;
            ImgReward.Start();

            TextMeshProUGUI InfoObjectives = PanelInfoMission.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            InfoObjectives.text = missionI.toStringObjectives(false);

            if (missionI.objectsAvailable > 0)
            {
                PanelObjects.SetActive(true);
                PrepareObjects();
            }
            else
            {
                PanelObjects.SetActive(false);
            }

            MMenu.MainM.Buttom_Do("mission_info");

            MissionInfo.MissionI.mission = missionI;
			MissionInfo.MissionI.actualMission = missionID;
			MissionInfo.MissionI.maxDistance_missil = 140;
			MissionInfo.MissionI.minDistance_missil = 120;
			MissionInfo.MissionI.ActualAssets = WorldManager.WorldM.worlds [missionI.worldID - 1].GameAssets;
            MissionInfo.MissionI.ActualBackgrounds = WorldManager.WorldM.worlds[missionI.worldID - 1].GameBackgrounds;

        if (UserInfo.UserI.missionsComplete.ContainsKey (missionI.worldID)) {
				if (UserInfo.UserI.missionsComplete [missionI.worldID].ContainsKey (missionID)) {
					MissionInfo.MissionI.info = UserInfo.UserI.missionsComplete [missionI.worldID] [missionID];
				}
			}
            ChangeUIWorldSelected(missionI.worldID);

            ChangeViewCamera(true);
	}

    public void StartGame() {
        bool reduct = RewardSystem.RewardS.CalculateFuelGame(-1);
        if (reduct)
        {
            if(MissionInfo.MissionI.mission.objectsAvailable > 0)
                MissionInfo.MissionI.objects_game = objects_game;
            LevelManager.LevelM.LoadScene("Game");
        }
    }

  
    public void MissionComplete(string missionID, int worldID){
		Dictionary<string,Dictionary<string,object>> newM = new Dictionary<string,Dictionary<string,object>> ();
		Dictionary<string,object> info = new Dictionary<string,object> ();

		info.Add ("score", GameManager.GameM.score);
		info.Add ("timesWon", 1);

		if (UserInfo.UserI.missionsComplete.ContainsKey (worldID)) {
			if(UserInfo.UserI.missionsComplete[worldID].ContainsKey(missionID)){
				AddOrUpdateInfo (UserInfo.UserI.missionsComplete [worldID] [missionID], info);
			}else{
				UserInfo.UserI.missionsComplete [worldID].Add (missionID,info);
			}
		} else {
			newM.Add (missionID,info);
			UserInfo.UserI.missionsComplete.Add (worldID, newM);
		}
	}

	public Dictionary<string,object> AddOrUpdateInfo(Dictionary<string,object> actualDict, Dictionary<string,object> newDict){

		foreach (string key in newDict.Keys) {
			if (key == "timesWon") {
				if (actualDict.ContainsKey (key)) {
					actualDict [key] = Convert.ToInt32 (actualDict [key]) + Convert.ToInt32 (newDict [key]);
				} else {
					actualDict.Add (key, newDict [key]);
				}
			} else {
				if(actualDict.ContainsKey(key)){
					if (key == "score") {
						if (Convert.ToInt32 (actualDict [key]) < Convert.ToInt32 (newDict [key])) {
							actualDict [key] = newDict [key];
						}
					} else {
						actualDict[key] = newDict[key];
					}
				}else{
					actualDict.Add (key, newDict[key]);
				}
			}
		} 
		return actualDict;
	}


	public void MissionsClear(){
		UserInfo.UserI.missionsComplete.Clear();
	}

	public void SaveAllData(){
		Serializer.serializer.SaveInfo ();
	}

    public void PrepareObjects()
    {
        for (int i = 0; i < ObjectsUI.transform.childCount; i++)
            Destroy(ObjectsUI.transform.GetChild(i).gameObject);

        foreach (string obj in UserInfo.UserI.objects.Keys)
        {
            try
            {
                if (UserInfo.UserI.objects[obj] > 0 && ObjectsManager.ObjectsM.objects[int.Parse(obj) - 1] != null)
                {

                    GameObject objectPrefab = (GameObject)Instantiate(ObjectPrefab, transform.position, Quaternion.identity);
                    objectPrefab.transform.SetParent(ObjectsUI.transform);
                    objectPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    objectPrefab.name = obj;
                    ObjectsManager.ObjectsM.InstantiateObject(int.Parse(obj), objectPrefab, UserInfo.UserI.objects[obj]);
                }
            }
            catch
            {
                //Si se hace un error, ignorar el objeto actual. Puede que no exista.
            }
        }
    }

    public void ChangeViewCamera(bool opc)
    {
        if (opc && !ViewChanged)
        {
            Animation anim = DecorationUI.GetComponent<Animation>();
            Animation camera = Camera.main.GetComponent<Animation>();

            anim["decorationUIM"].speed = 1;
            anim["decorationUIM"].time = 0.0f;
            anim.Play("decorationUIM");

            camera["CameraMoveDown"].speed = 1;
            camera["CameraMoveDown"].time = 0.0f;
            camera.Play("CameraMoveDown");

            ViewChanged = true;

        }
        else
        {
            if (!opc && ViewChanged)
            {
                Animation anim = DecorationUI.GetComponent<Animation>();
                Animation camera = Camera.main.GetComponent<Animation>();

                anim["decorationUIM"].speed = -1;
                anim["decorationUIM"].time = anim["decorationUIM"].length;
                anim.Play("decorationUIM");

                camera["CameraMoveDown"].speed = -1;
                camera["CameraMoveDown"].time = camera["CameraMoveDown"].length;
                camera.Play("CameraMoveDown");

                ViewChanged = false;
            }
        }
    }
}