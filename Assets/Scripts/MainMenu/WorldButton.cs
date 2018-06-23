using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldButton : MonoBehaviour {

	public int worldID;

	public void SelectWorld(){
		MissionsManager.MissionsM.MissionsCreated = false; ///CAMBIAR!
		MissionsManager.MissionsM.LoadMissionsData (worldID);
		MMenu.MainM.Buttom_Do ("missions");
	}
}
