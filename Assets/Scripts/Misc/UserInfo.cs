using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ObjectType;

public class UserInfo : MonoBehaviour {


	public static UserInfo UserI;

	public int carrots;
	public int fuelGame;
	public int exp;
	public Dictionary<int, Dictionary<string,Dictionary<string,object>>> missionsComplete = new Dictionary<int, Dictionary<string,Dictionary<string,object>>>();
	public DateTime LastFuelTimer;
	public Dictionary<string,int> objects = new Dictionary<string, int>();
    public Dictionary<string, int> vehicles = new Dictionary<string, int>();
    public Dictionary<string, object> bunny = new Dictionary<string, object>();


    // Use this for initialization
    void Awake () {
		UserI = this;
	}


    public bool UserHasObject(int ID, OType type)
    {
        bool exist = false;
        if (type == OType.Boost)
            exist = objects.ContainsKey(ID.ToString());
        else if (type == OType.Vehicle)
            exist = vehicles.ContainsKey(ID.ToString());
        return exist;
    }

    public int CountObject(int ID, OType type)
    {
        int count = 0;
        if (type == OType.Boost)
            count = UserInfo.UserI.objects[ID.ToString()];
        else if (type == OType.Vehicle)
            count = UserInfo.UserI.vehicles[ID.ToString()];
        return count;
    }

    public Dictionary<string, int> GetVehicles() {
        return vehicles;
    }

}
