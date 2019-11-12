using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayerLevelSystem : MonoBehaviour {

	public static PlayerLevelSystem PLevelS;
	int C = 50;
	public Slider EXPSlider;
	public int ActualEXP;
	public int ActualLevel;
	public float newEXP;
	public TextMeshProUGUI TextLevel;
	public GameObject LevelUpUI;
	public GameObject LevelUpImage;
	public Sprite CarrotImg;

	public int necesaryEXP;
	public AudioClip EXPSound;
	public AudioClip EXPSound2;
	public AudioClip EXPSound3;
	public Animation UIEXP;

	Dictionary<string, object> newObjects = new Dictionary<string, object> ();
	private double oldLevel;
	private double newLevel;

	private bool LevelUpActivated = false;

	public bool UpdateExp;
	public bool PlaySound;
	public bool UIEXPEnable;
	public bool StartVisibility;

	void Awake () {
		PLevelS = this;
	}

	// Use this for initialization
	void Start () {
		ActualEXP = UserInfo.UserI.exp;
		UIEXPEnable = false;
		LevelUpUI.SetActive (false);
		UpdateLevel (false);
		if(StartVisibility)
			ActivateUIEXP (true);
		else
			ActivateUIEXP (false, false);
		
	}


	public List<double>  CalculateLevel(int exp = 0){
		List<double> infoExp = new List<double> ();

		double level = Math.Truncate((C + Math.Sqrt (C * C - 4 * C * (-exp) )) / (C*2));  //Formula para obtener el número de nivel dependiendo de la exp.
		infoExp.Add(level);

		double diff = C * level  * level  - C * level ;  //Formula para obtener la diferencia de la exp actual a la neceesaio para el nivel en que esta.
		infoExp.Add(diff);

		double next = C * (level+1)  * (level+1)  - C * (level+1);  //Formula para obtener la exp necesaria para el siguiente nivel.
		infoExp.Add(next);    

		return infoExp; //Level, Difference, Next Exp

	}


	public int CalculateMinEXPLevel(int level = 0){
		return C * (level)  * (level)  - C * (level); //Level, Difference, Next Exp
	}

	public void ModifiEXP(int newExp){
		LevelUpActivated = false;
		List<double> aLevel = CalculateLevel (UserInfo.UserI.exp);
		UserInfo.UserI.exp += newExp;
		ActualEXP += newExp;
		List<double> nLevel = CalculateLevel (UserInfo.UserI.exp);
		StartCoroutine ("StartModif");
		if (aLevel [0] < nLevel [0]) {
			LevelUp (aLevel [0], nLevel [0]);
			oldLevel = aLevel [0];
			newLevel = nLevel [0];
		}
		if (!UIEXPEnable)
			ActivateUIEXP (true);
	}

	private void LevelUp(double oldLevel,double newLevel){
		int carrots = 0;
		Dictionary<string, object> objects = new Dictionary<string, object> ();
		Dictionary<int, int> booster = new Dictionary<int, int> ();


		for (int i = 0; i < newLevel - oldLevel; i++) {
			carrots += 10;

			if (booster.ContainsKey (1)) {
				booster [1] += 2;
			} else {
				booster.Add (1, 2);
			}

			if (booster.ContainsKey (2)) {
				booster [2] += 2;
			} else {
				booster.Add (2, 2);
			}
		}
		if (carrots > 0) {
			objects.Add ("carrots", carrots);
		}

		if (booster.Count > 0) {
			objects.Add("booster",booster);
		}

		RewardSystem.RewardS.UpdateInventory (objects);

		if(newObjects.ContainsKey("carrots")){
			newObjects["carrots"] = Convert.ToInt32(newObjects["carrots"]) + carrots;
		}else{
			newObjects.Add ("carrots", carrots);
		}
			
		Dictionary<int, int> newbooster = new Dictionary<int, int> ();
		if (newObjects.ContainsKey ("booster")) {
			newbooster = newObjects ["booster"] as Dictionary<int, int>;

			foreach (int boost in booster.Keys) {
				if (newbooster.ContainsKey (boost)) {
					newbooster [boost] += booster [boost];
				} else {
					newbooster.Add (boost, booster [boost]);
				}
			}
			newObjects ["booster"] = newbooster;
		} else {
			newObjects.Add ("booster", booster);
		}


	}

	IEnumerator StartLevelUp(){
		LevelUpUI.transform.GetChild (5).gameObject.SetActive (false);
		LevelUpUI.transform.GetChild (4).gameObject.SetActive (false);
		LevelUpUI.SetActive (true);
		LevelUpUI.transform.GetChild (2).GetComponent<TextMeshProUGUI> ().text = oldLevel.ToString();
		yield return new WaitForSeconds(0.2f);

		for(int i = 0; i < LevelUpUI.transform.GetChild (4).gameObject.transform.childCount; i++)
			Destroy(LevelUpUI.transform.GetChild (4).transform.GetChild(i).gameObject);
		
		foreach(var reward in newObjects){
			switch (reward.Key) {
			case "carrots":
				GameObject UIRewardPrefab1 = (GameObject)Instantiate (LevelUpImage, transform.position, Quaternion.identity);

				UIRewardPrefab1.transform.SetParent (LevelUpUI.transform.GetChild (4).gameObject.transform, false);
				UIRewardPrefab1.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
                ObjectsManager.ObjectsM.InstantiateCarrotObject(UIRewardPrefab1, int.Parse(reward.Value.ToString()));

				break;
			case "booster":
				Dictionary<int,int> listv = reward.Value as Dictionary<int,int>;
				foreach(var booster in listv){
					GameObject UIRewardPrefab2 = (GameObject)Instantiate (LevelUpImage, transform.position, Quaternion.identity);

					UIRewardPrefab2.transform.SetParent (LevelUpUI.transform.GetChild (4).gameObject.transform,false);
					UIRewardPrefab2.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
                    ObjectsManager.ObjectsM.InstantiateObject(booster.Key, UIRewardPrefab2, booster.Value);
				}
				break;
			}

		}
		newObjects.Clear ();
		yield return new WaitForSeconds(0.4f);
		LevelUpUI.transform.GetChild (4).gameObject.SetActive (true);
		LevelUpUI.transform.GetChild (2).GetComponent<TextMeshProUGUI> ().text = newLevel.ToString();
		yield return new WaitForSeconds(0.2f);
		LevelUpUI.transform.GetChild (5).gameObject.SetActive (true);
	}

	IEnumerator StartModif(){

		yield return new WaitForSeconds(1.5f);
		if (!UpdateExp) {
			SoundManager.SoundM.StartAudioEXP (EXPSound, 1.0f);
		}
		UpdateExp = true;
	}

	public void UpdateLevel(bool effects){
		List<double> infoExp;
		List<double> infoExp2;
		infoExp = CalculateLevel(ActualEXP);
		if (effects) {
			infoExp2 = CalculateLevel(Convert.ToInt32 (newEXP));
			if (newEXP < ActualEXP) {


				if(newEXP - Convert.ToInt32 (infoExp2 [1])  <=  Convert.ToInt32 (infoExp2 [2]) - Convert.ToInt32 (infoExp2 [1])){ // Si hay exp a añadir a la barra:
					EXPSlider.value += 200 * Time.deltaTime;
					newEXP += 200 * Time.deltaTime;
					SoundManager.SoundM.AudioBackEXP.mute = false;
				}

			} else { //Establecer valores seguros al finalizar
				newEXP = ActualEXP;
				EXPSlider.value = newEXP - Convert.ToInt32(infoExp [1]);
				UpdateExp = false;
				Debug.Log ("Desactivando exp.");
				SoundManager.SoundM.AudioBackEXP.Stop ();

			}

			if (necesaryEXP < Convert.ToInt32 (infoExp2 [2]) - Convert.ToInt32 (infoExp2 [1])) { // Para subir indicar un nivel más y el reinicio de la barra
				TextLevel.text = (ActualLevel + 1).ToString ();
				EXPSlider.maxValue = Convert.ToInt32 (infoExp2 [2]) - Convert.ToInt32 (infoExp2 [1]);
				EXPSlider.value = 0;
				ActualLevel += 1;
				necesaryEXP = Convert.ToInt32 (infoExp2 [2]) - Convert.ToInt32 (infoExp2 [1]);
				SoundManager.SoundM.StartAudio (EXPSound2, 0.5f);
				Debug.Log ("NUEVO NIVEL!");

				if (LevelUpActivated == false) {
					LevelUpActivated = true;
					StartCoroutine ("StartLevelUp");
				}
			}


		}else{
			EXPSlider.maxValue = Convert.ToInt32(infoExp [2]) - Convert.ToInt32(infoExp [1]);
			EXPSlider.value = ActualEXP - Convert.ToInt32(infoExp [1]);
			TextLevel.text = infoExp [0].ToString();
			newEXP = ActualEXP;
			ActualLevel = Convert.ToInt32(infoExp [0]);
			necesaryEXP = Convert.ToInt32 (infoExp [2]) - Convert.ToInt32 (infoExp [1]);
		}
		
	}

	public void ActivateUIEXP(bool opc, bool show = true){
		if (opc) {
			UIEXP ["UIEXP"].speed = -1;
			UIEXP["UIEXP"].time = UIEXP ["UIEXP"].length;
			UIEXP.Play ("UIEXP");
			UIEXPEnable = true;
		} else {
            UIEXP["UIEXP"].speed = 1;
            if (show)
                UIEXP["UIEXP"].time = 0.0f;
            else
                UIEXP["UIEXP"].time = UIEXP["UIEXP"].length;
			UIEXP.Play ("UIEXP");
			UIEXPEnable = false;
		}

	}

	// Update is called once per frame
	void Update () {
		if (UpdateExp) {
			UpdateLevel (true);
		}
	}
}
