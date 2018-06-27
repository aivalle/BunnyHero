using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo : MonoBehaviour {

	public static MissionInfo MissionI;

	public int ActualMission;
	public int worldID;
	public string desc;
	public int max_tiempo;
	public int max_distance;
	public int max_golpes;
	public int RewardEXP;
	public Dictionary <int,int> rewards = new Dictionary <int,int>();
	public int maxDistance_missil;
	public int minDistance_missil;
	public int hitMode;
	public int objectsAvaliable;
	public int final_reward;
	public List<GameObject> ActualAssets = new List<GameObject>();

	// Use this for initialization
	void Awake () {
		MissionI = this;	
	}

}
