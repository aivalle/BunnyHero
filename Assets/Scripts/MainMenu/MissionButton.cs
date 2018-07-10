using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionButton : MonoBehaviour {

	public int missionID;

	//Cuando un botón para jugar una misión es pulsado:
	public void Pushed()
	{
		List<MissionID> mission = new List<MissionID>();
		if (missionID > 0) {
			MissionsManager.MissionsM.SelectMission (missionID);
		} else {           
			mission.Add (new MissionID (0,1,"infinite_desc", -1,2, -1, 20, new Dictionary <int,int>(){}, 2, 1, 1,-1));   //Add alternaative mission   ID,"desc",time,hits,distance, ewardExp, rewardId, rewardQuantity, hitmode, objectsAvailable, force final rewards
			MissionsManager.MissionsM.SelectMission (0, mission); //Modo de juego infinito (provicional)
		}
	}

}
