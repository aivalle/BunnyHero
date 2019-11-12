using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionButton : MonoBehaviour {

	public string missionID;
	public int worldID = 0;

	//Cuando un botón para jugar una misión es pulsado:
	public void Pushed()
	{
		MissionsManager.MissionsM.SelectMission (missionID, worldID);
	}

}
