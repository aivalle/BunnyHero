using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class RewardSystem : MonoBehaviour {

	public static RewardSystem RewardS;
	public TextMeshProUGUI carrotsText;
	private int limitFuelGame = 5;
	public TextMeshProUGUI FuelGameTimer;
	public TextMeshProUGUI FuelGameText;
	public GameObject WithOutFuel;
	private int secondsPerFuel = 60;

	void Awake(){
			RewardS = this;

	}
		
	// Use this for initialization
	void Start () {
		carrotsText.text = UserInfo.UserI.carrots.ToString();
		WithOutFuel.SetActive (false);
		CheckTimerFuelHistory ();
		if (FuelGameText) {
			FuelGameText.text = string.Format ("<gradient=\"Gradient_orange\">{0}</gradient>/{1}", UserInfo.UserI.fuelGame, limitFuelGame);
		}

		if (UserInfo.UserI.fuelGame >= limitFuelGame) {
			if (FuelGameTimer) {
				FuelGameTimer.text = "Lleno";
			}
		} 

	}

	public void CalculateCarrots(int amount){
		UserInfo.UserI.carrots += amount;
		RewardSystem.RewardS.carrotsText.text = UserInfo.UserI.carrots.ToString();
	}

	public void CalculateObjects(int ID_object, int amount){

		if (ID_object != 0) {
			string ID = ID_object.ToString ();
			if (UserInfo.UserI.objects.ContainsKey (ID)) {	

				UserInfo.UserI.objects [ID] = UserInfo.UserI.objects [ID] + amount;
			} else {
				UserInfo.UserI.objects.Add (ID, amount);
			}
		}
	}

	public void UpdateInventory (Dictionary<string,object> objects, GameObject text = null){

		foreach (var keyO in objects) {

			switch (keyO.Key) {
			case "carrots":
				CalculateCarrots (int.Parse(keyO.Value.ToString()));
				break;
			case "exp":
				PlayerLevelSystem.PLevelS.ModifiEXP (int.Parse(keyO.Value.ToString()));
				break;
			case "booster":
				Dictionary<int,int> listv = keyO.Value as Dictionary<int,int>;
				foreach(var reward in listv){
					CalculateObjects (reward.Key,reward.Value);
				}
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		TimerFuelGame ();
	}

	public string TimerCount(int seconds){

		return String.Format("{0:0}:{1:00}", Mathf.Floor(seconds/60), seconds % 60);
	}

	public int getSecondsDiff(DateTime Old) {

		//Get the current time
		DateTime Now = DateTime.UtcNow;

		//Convert both to seconds
		int targetSec = Old.Hour * 60 * 60 + Old.Minute * 60 + Old.Second;
		int nowSec = Now.Hour * 60 * 60 + Now.Minute * 60 + Now.Second;

		//Get the difference in seconds
		int diff = nowSec - targetSec;

		return diff;
	}
		

	public void TimerFuelGame(){

		if (UserInfo.UserI.fuelGame < limitFuelGame) {
			int time = getSecondsDiff (UserInfo.UserI.LastFuelTimer);

			if (time >= secondsPerFuel) {
				UserInfo.UserI.LastFuelTimer = DateTime.UtcNow;
				CalculateFuelGame (1);

			} else {
				if (FuelGameTimer) {
					FuelGameTimer.text = TimerCount (secondsPerFuel - time);
				}
			}
		}
	}

	public bool CalculateFuelGame(int amount){
		int lastFuel = UserInfo.UserI.fuelGame;
		if (UserInfo.UserI.fuelGame + amount >= 0) {
			UserInfo.UserI.fuelGame += amount;


			if (UserInfo.UserI.fuelGame == limitFuelGame - 1 && lastFuel == limitFuelGame) {
				UserInfo.UserI.LastFuelTimer = DateTime.UtcNow;
			}

			if (UserInfo.UserI.fuelGame >= limitFuelGame) {
				if (FuelGameTimer) {
					FuelGameTimer.text = "Lleno";
				}
			} 

			if (FuelGameText) {
				FuelGameText.text = string.Format ("<gradient=\"Gradient_orange\">{0}</gradient>/{1}", UserInfo.UserI.fuelGame, limitFuelGame);
			}
			return true;
		} else {
			WithOutFuel.SetActive (true);
			Debug.Log ("Gasolina insuficiente.");
			return false;
		}

	}


	public void CheckTimerFuelHistory(){

		DateTime timesaved = UserInfo.UserI.LastFuelTimer;
		DateTime timenow = DateTime.UtcNow;

			for (int i = 1; i <= limitFuelGame; i++) {
				if (UserInfo.UserI.fuelGame < limitFuelGame) {
				DateTime timenext = timesaved.AddSeconds (secondsPerFuel);
				int compare = DateTime.Compare (timenext, timenow);
					if (compare < 0) {
					CalculateFuelGame (1);
					timesaved = timesaved.AddSeconds (secondsPerFuel);
					} else {
					UserInfo.UserI.LastFuelTimer = timesaved;
					break;
					}
				} else {
					break;
				}
			}

	}

}
