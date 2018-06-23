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

	public int necesaryEXP;
	public AudioClip EXPSound;
	public AudioClip EXPSound2;
	public AudioClip EXPSound3;
	public Animation UIEXP;


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
		UpdateLevel (false);
		if(StartVisibility)
			ActivateUIEXP (true);
		else
			ActivateUIEXP (false);
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
		UserInfo.UserI.exp += newExp;
		ActualEXP += newExp;
		StartCoroutine ("StartModif");
		if (UIEXPEnable == false)
			ActivateUIEXP (true);
	}

	IEnumerator StartModif(){

		yield return new WaitForSeconds(1.5f);
		if (UpdateExp == false) {
			SoundManager.SoundM.StartAudioEXP (EXPSound, 1.0f);
		}
		UpdateExp = true;
	}

	public void UpdateLevel(bool effects){
		List<double> infoExp;
		List<double> infoExp2;
		infoExp = CalculateLevel(ActualEXP);
		if (effects == true) {
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

	public void ActivateUIEXP(bool opc){
		if (opc) {
			UIEXP ["UIEXP"].speed = -1;
			UIEXP["UIEXP"].time = UIEXP ["UIEXP"].length;
			UIEXP.Play ("UIEXP");
			UIEXPEnable = true;
		} else {
			UIEXP ["UIEXP"].speed = 1;
			UIEXP ["UIEXP"].time = 0.0f;
			UIEXP.Play ("UIEXP");
			UIEXPEnable = false;
		}

	}

	// Update is called once per frame
	void Update () {
		if (UpdateExp == true) {
			UpdateLevel (true);

		}
	}
}
