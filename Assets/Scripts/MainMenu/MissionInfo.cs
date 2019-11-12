using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionInfo : MonoBehaviour {

	public static MissionInfo MissionI;

	public string actualMission;
	public int maxDistance_missil;
	public int minDistance_missil;

	public Dictionary <string,object> info = new Dictionary <string,object>();
	public List<GameObject> ActualAssets = new List<GameObject>();
    public List<GameObject> ActualBackgrounds = new List<GameObject>();

    public MissionID mission;
    public List<int> objects_game = new List<int>();

    // Use this for initialization
    void Awake () {
		MissionI = this;	
	}

}
