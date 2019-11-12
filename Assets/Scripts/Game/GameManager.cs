using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;


public class GameManager : MonoBehaviour {
	
	public static GameManager GameM;

	//UI Objects
	
	public GameObject UI_user_normal;
	public TextMeshProUGUI TextCount;
	public TextMeshProUGUI TextDistance;
	public TextMeshProUGUI TextDistance2;
	public TextMeshProUGUI TextCarrots;
	public TextMeshProUGUI TextCountDown;
	public Slider SliderDistance;
	public Slider SliderFuel;
	public Slider SliderHeal;
	public GameObject PollosUI;
	public GameObject GameOverUI;
	public GameObject PauseUI;
	public GameObject ImportantUI;
	public GameObject ObjectsUI;
	public GameObject ObjectsGameUI;
	public GameObject FinalMessage; 

	public Animation AnimUIUser;
	public GameObject PollosPrefab;
    public GameObject BackgroundsGO;

	// Target
	public GameObject target;
	private Rigidbody2D rb;
	public CircleCollider2D CircleColl2D;

	// Game status
	public bool StartGame;
	public bool RunGame;
	public bool GamePause;
	public bool EndGame;

	// Info mission
	public string actual_mission;
	public string actual_desc;
	public float actual_time;
	public float actual_distance;
	public int actual_hits;
	public int actual_worldID;
	public int actual_maxDistance_missil;
	public int actual_minDistance_missil;
	public int actual_hitMode;
	public int actual_objectsAvailable;
	public int actual_multiplier_exp;
	public int final_reward;
	public int actual_max_reward_user;
	public Dictionary <int,int> rewards = new Dictionary <int,int>();
    public List<int> objects_game = new List<int>();

    //Info actual game
    public int hits;
	public float fuel;
	public int carrots;
	public float time;
	public float distance;
	private float distanceVar;
	public int score;

	// Costs
	public float clickfuel;
	public float secondfuel;

	// Objects variables
	public List<int> CurrentObject = new List<int> ();

	private Camera actualCamera;

	void Awake(){
		GameM = this;
		Missions_In_Game ();
	}

	// Use this for initialization
	void Start () {
        actualCamera = Camera.main;
		StartCoroutine(SoundManager.SoundM.MusicBackDecreaseVolume ());
		BunnyManager.BunnyM.shieldBunny.SetActive (false);
		BunnyManager.BunnyM.MagnetEffect.SetActive (false);
		CircleColl2D = target.GetComponent<CircleCollider2D> ();
		rb = target.GetComponent<Rigidbody2D> ();
		GamePause = false;
		GameOverUI.SetActive (false);
		PauseUI.SetActive (false);
		ImportantUI.SetActive (false);
		FinalMessage.SetActive (false);
		UI_user_normal.SetActive (false);
		actualCamera.orthographicSize = 10f;

		fuel = 100;
		SliderFuel.value = fuel;
		AnimUIUser = UI_user_normal.GetComponent<Animation>();

		if (actual_distance < 0) {
			TextDistance2.gameObject.SetActive (true);
			SliderDistance.gameObject.SetActive(false);
			TextDistance.gameObject.SetActive (false);
			TextDistance2.text = "0";
		} else {
			TextDistance2.gameObject.SetActive (false);
			SliderDistance.gameObject.SetActive(true);
			TextDistance.gameObject.SetActive (true);
			TextDistance.text = string.Format ("0/{0} <gradient=\"Gradient_orange\">m</gradient>",  actual_distance);
			SliderDistance.maxValue = actual_distance;
		}
			

		if (actual_hitMode == 1) {
			PollosUI.SetActive (true);
			SliderHeal.gameObject.SetActive (false);
		} else {
			PollosUI.SetActive (false);
			SliderHeal.gameObject.SetActive (true);
		}
			
		SliderHeal.maxValue = actual_hits;
		SetHits (actual_hits);
		ChangeMissilRate (actual_minDistance_missil, actual_maxDistance_missil);

        SelectedObjects();
        StartMission();

    }


	// Update is called once per frame
	void Update () {
		if (!GamePause) {
			if (actual_time < 0) {
				StartCountTime (true);
			} else {
				StartCountTime (false);
			}

			if (RunGame) {
				GameManager.GameM.CalculateFuel (0);
			}
			CalculateDistance ();
			IncreaseCameraSize ();
		}
	}


	public void StartMission(){
		StartGame = true;
		UI_user_normal.SetActive (true);
		AnimUIUser.Play ("StartUIGame");
	}

	public void Missions_In_Game(){

		actual_mission = MissionInfo.MissionI.actualMission;
		actual_desc = MissionInfo.MissionI.mission.desc;
		actual_time = MissionInfo.MissionI.mission.time;
		actual_distance = MissionInfo.MissionI.mission.distance;
		actual_hits = MissionInfo.MissionI.mission.hits;
		actual_hitMode = MissionInfo.MissionI.mission.hitMode;
		actual_multiplier_exp = MissionInfo.MissionI.mission.multiplierEXP;
		rewards = MissionInfo.MissionI.mission.rewards;

		actual_minDistance_missil = MissionInfo.MissionI.minDistance_missil;
		actual_maxDistance_missil = MissionInfo.MissionI.maxDistance_missil;
		actual_objectsAvailable = MissionInfo.MissionI.mission.objectsAvailable;
		final_reward = MissionInfo.MissionI.mission.final_reward;
		actual_worldID = MissionInfo.MissionI.mission.worldID;
		actual_max_reward_user = MissionInfo.MissionI.mission.max_reward_user;
		target.GetComponent<SceneCreator> ().availableRooms = MissionInfo.MissionI.ActualAssets;
        objects_game = MissionInfo.MissionI.objects_game;
        foreach (GameObject go in MissionInfo.MissionI.ActualBackgrounds)
        {
            GameObject background = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity);
            background.transform.SetParent(BackgroundsGO.transform, false);
        }

        GameObject actualRoom = (GameObject)Instantiate (MissionInfo.MissionI.ActualAssets[0], new Vector3(-9.5f, 0, 0), Quaternion.identity);
		target.GetComponent<SceneCreator> ().currentRooms.Add (actualRoom);


		if (actual_time < 0) {
			time = 0;
		} else {
			time = actual_time;
		}
		TextCount.text = time.ToString();
	}

	public void StartCountTime(bool infinite){

		if (RunGame) {

			if (actual_time <= 0 && !infinite) {
				TextCount.text = "0";
				Debug.Log ("Tiempo terminado");
				StartCoroutine("GameOver",false);
			} else {
				if (!infinite) {
					time -= Time.deltaTime;
					TextCount.text = Convert.ToInt32(time).ToString();	
				}else{
					time += Time.deltaTime;
					TextCount.text = Convert.ToInt32(time).ToString();	
				}
			}
		}
	}

	public void CalculateDistance(){
		if (RunGame) {
			float CalcDistance = Time.deltaTime * rb.velocity.x * 0.35f;
			distance += CalcDistance;
			distanceVar += CalcDistance;

			if (actual_distance == -1) {
				TextDistance2.text = string.Format ("{0} <gradient=\"Gradient_orange\">m</gradient>", Convert.ToInt32 (distance));
			} else {
				SliderDistance.value = distance;
				TextDistance.text = string.Format ("{0}/{1} <gradient=\"Gradient_orange\">m</gradient>", Convert.ToInt32 (distance), actual_distance);
			}
				

			if(Convert.ToInt32 (distance) >=  actual_distance && actual_distance != -1){
				StartCoroutine("GameOver",true);
			}

			if (distanceVar >= 250) {
				distanceVar = 0;
				ChangeMissilRate(actual_minDistance_missil - 10, actual_maxDistance_missil - 10);
			}
		}
	}


	public void CalculateFuel(int opc){

		float consume = 0;
		if (opc == 0) { //Gasto de gasolina por segundo
			consume = secondfuel;
		} else if (opc == 1) { //Gasto de gasolina por click
			consume = clickfuel;
		} else if (opc == 2) { //Añadir gasolina
			consume = 1000;
		}

		float varble = Time.deltaTime;
		fuel += varble * consume;

		if (fuel > 100) {
			fuel = 100;
		} else if (fuel <= 0) {
			fuel = 0;
			BunnyManager.BunnyM.vehicleInfo.ActivateEffect(false);
            if (BunnyManager.BunnyM.grounded == true && rb.velocity.x <= 2f) {
					StartCoroutine("GameOver",false);
			}
		}
			
		SliderFuel.value = fuel;
	}

	public void SetHits(int hits_get){
		hits += hits_get; 
		if (actual_hitMode == 1) {//Si el modo actual de choque es de pollos.
			if (hits > 0) {
				PollosUI.transform.GetChild (0).GetComponent<TextMeshProUGUI> ().text = string.Format ("x<gradient=\"Gradient_green\">{0}</gradient>", hits);
				if (RunGame == true) {
					Vector2 targetV = new Vector2 (target.transform.position.x - 1f, target.transform.position.y);
					Instantiate (PollosPrefab, targetV, Quaternion.Inverse (this.transform.rotation));
				}

			} else {
				Debug.Log ("Golpes hechos");
				StartCoroutine("GameOver",false);
			}
		} else { // El modo actual es de vida del vehiculo.
			if (hits > 0) {
				SliderHeal.value = hits;
			} else {
				Debug.Log ("Golpes hechos");
				StartCoroutine("GameOver",false);
			}
		}

	}

	public void ChangeMissilRate(int min, int max){
		if (max >= min ) {
			target.GetComponent<MissileCreator> ().objectsMaxDistance = max;
			actual_maxDistance_missil = max;
			if (min >= 40) {
				target.GetComponent<MissileCreator> ().objectsMinDistance = min;
				actual_minDistance_missil = min;
			}
		}
	}
		
	public void CalculateCarrotsGame(int opc){
		if (RunGame) {
			int carrot = 0;
			if (opc == 0) {//Añadir 1 zanahoria
				carrot = 1;
			}
			carrots += carrot;
			TextCarrots.text = carrots.ToString ();
		}
	}

	public void CalculateScore(){
		score += (int)(distance * distance) / 1000;
		score += (int)((time * time)+150) / 70;
		score += (int)hits * hits * 50;
		score += carrots * 2;
	}


	public IEnumerator GameOver(bool success){
		BunnyManager.BunnyM.vehicleInfo.ActivateEffect(false);
        EndGame = true;
		RunGame = false;
		BunnyManager.BunnyM.Click = false;
		StartCoroutine(SoundManager.SoundM.MusicBackDecreaseVolume ());
		CalculateScore ();
		if (final_reward == 1 || success) {
			FinalMessage.SetActive (true);
			FinalMessage.gameObject.transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = string.Format ("{0} {1}", "Puntuación: ", score);
			yield return new WaitForSeconds (1f);
		} else {
			BunnyManager.BunnyM.StartCoroutine ("EjectBox");
		}
		yield return new WaitForSeconds (0.25f);
		CameraMovement.CameraM.Follow = false;
		yield return new WaitForSeconds (1f);
		FinalMessage.SetActive (false);

		double actual_exp = score * 0.1 * actual_multiplier_exp;
		if (final_reward == 1 || success) {
			//Give rewards
			Dictionary<string,object> list = new Dictionary<string,object> ();
			list.Add ("carrots", carrots);
			list.Add ("exp", (int)actual_exp);

			Dictionary<int,int> list2 = new Dictionary<int,int> ();
			foreach (var rewardID in rewards) {
				list2.Add (rewardID.Key,rewardID.Value);
			}
			list.Add ("booster", list2);

			if (MissionInfo.MissionI.info.ContainsKey ("timesWon")) {
				if (Convert.ToInt32 (MissionInfo.MissionI.info ["timesWon"]) < actual_max_reward_user || actual_max_reward_user == -1) {
					RewardSystem.RewardS.UpdateInventory (list);
				} else {
					rewards.Clear ();
				}
			} else {
				RewardSystem.RewardS.UpdateInventory (list);
			}

			//UI
			GameOverUI.SetActive (true);
			GameOverUI.transform.GetChild(0).gameObject.SetActive (true);
			GameOverUI.transform.GetChild (0).transform.GetChild (0).transform.GetChild (2).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = string.Format ("{0}", score);
			GameOverUI.transform.GetChild (0).transform.GetChild (0).transform.GetChild (3).transform.GetChild (1).GetComponent<TextMeshProUGUI> ().text = string.Format ("{0} <sprite name=\"icon_carrot\"> {1} <gradient=\"Gradient_green\"><size=75%>EXP</size></gradient>",  carrots, (int)actual_exp);

            GameOverUI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = MissionInfo.MissionI.mission.toStringObjectives(true);


            if (rewards.Count > 0) {
				GameOverUI.transform.GetChild (0).transform.GetChild (0).transform.GetChild (4).transform.GetChild (1).gameObject.SetActive (true);
				ScrollSprite ImgReward = GameOverUI.transform.GetChild (0).transform.GetChild (0).transform.GetChild (4).transform.GetChild (1).transform.GetChild (1).GetComponent<ScrollSprite> ();
				List<GameObject> listSprites = new List<GameObject> ();
				foreach (var rewardID in rewards) {
					listSprites.Add (ObjectsManager.ObjectsM.InstantiateObject(rewardID.Key, ImgReward.gameObject, rewardID.Value));
				}
				ImgReward.listSprites = listSprites;
				ImgReward.Start();
			} else {
				GameOverUI.transform.GetChild (0).transform.GetChild (0).transform.GetChild (4).transform.GetChild (1).gameObject.SetActive (false);
			}
			
			MissionsManager.MissionsM.MissionComplete (actual_mission, actual_worldID);
			Serializer.serializer.SaveInfo ();
		} else {
			SoundManager.SoundM.StartCoroutine ("MusicBackDecrease");
			GameOverUI.SetActive (true);
			GameOverUI.transform.GetChild(1).gameObject.SetActive (true);
		}

		ImportantUI.SetActive (true);
		UI_user_normal.SetActive (false);
		ObjectsGameUI.SetActive (false);
	}

	public void SelectedObjects(){
		int i = 0;
		foreach(int obj in objects_game){
			if (objects_game[i] > 0) {
				Transform newobj = ObjectsGameUI.transform.GetChild (i);
				newobj.gameObject.SetActive (true);
                ObjectsManager.ObjectsM.InstantiateObject(int.Parse(obj.ToString()), newobj.gameObject);
                newobj.name = obj.ToString();
				i++;
			}
		}
	}


	public IEnumerator ShowBigMessage(){
		yield return new WaitForSeconds (1f);
	}

	//Permite hacer un zoomOut cuando empieza la partida.
	public void IncreaseCameraSize(){

		if(actualCamera.orthographicSize < 15 && RunGame){
			float pointpersecond = 2.5f;
			actualCamera.orthographicSize += pointpersecond * Time.deltaTime;
		}
	}
		


	public void ButtonsGame(string show_option){
		switch (show_option)
		{
		case "reset":
			bool reduct = RewardSystem.RewardS.CalculateFuelGame (-1);
			if(reduct == true){
				LevelManager.LevelM.LoadScene ("Game");
			}
			break;
		case "main_menu":
			LevelManager.LevelM.LoadScene ("Menu");
			break;
		default:
			Debug.Log("No se encuentra el menu " + show_option);
			break;
		}
	}

	public void PauseGame(bool opc){
		if (opc) {
			BunnyManager.BunnyM.Click = false;
			ObjectsGameUI.SetActive (false);
			UI_user_normal.SetActive (false);
			ImportantUI.SetActive (true);
			GamePause = true;
			PauseUI.SetActive (true);
			Debug.Log("Juego pausa");
			CameraMovement.CameraM.Follow = false;
		} else {
			BunnyManager.BunnyM.Click = true;
			ImportantUI.SetActive (false);
			PauseUI.SetActive (false);
			StartCoroutine ("PauseCountDown",3);
			CameraMovement.CameraM.Follow = true;
		}
	}

	public IEnumerator PauseCountDown(int duration){
		int newtime = 0;
		while (newtime < duration) {
			TextCountDown.text = (duration - newtime).ToString ();
			TextCountDown.gameObject.SetActive (true);
			yield return new WaitForSeconds (1f);
			TextCountDown.gameObject.SetActive (false);
			newtime++;
		}
		ObjectsGameUI.SetActive (true);
		UI_user_normal.SetActive (true);
		AnimUIUser.Play ("StartUIGame");
		GamePause = false;
		Debug.Log("Juego resumen");
	}

}
