using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UserInfo : MonoBehaviour {


	public static UserInfo UserI;

	public int carrots;
	public int fuelGame;
	public int exp;
	public Dictionary<string, List<int>> missionsComplete = new Dictionary<string, List<int>>();
	public DateTime LastFuelTimer;
	public Dictionary<string,int> objects = new Dictionary<string, int>();


	// Use this for initialization
	void Awake () {
		UserI = this;
	}
		
}
