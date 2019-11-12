using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WorldManager : MonoBehaviour {

	public static WorldManager WorldM;

	public List<WorldID> worlds = new List<WorldID>();

	public GameObject worldsPrefab;
	public GameObject worldList;
	public List<GameObject> UIMenu = new List<GameObject>();
	public List<Sprite> UIMenuBtn = new List<Sprite>();

	public List<GameObject> World1Assets;
	public List<GameObject> World2Assets;

    public List<GameObject> World1Backgrounds;
    public List<GameObject> World2Backgrounds;


    void Awake () {
		WorldM = this;
	}

	void CreateWorlds(){

		//Add missions            ID,"desc",time,hits,distance, rewardId, rewardQuantity, hitmode, objectsAvailable, force final rewards
		worlds.Add( new WorldID(1,"the_farm","the_farm_desc",15, UIMenu[0],0,World1Assets, World1Backgrounds));
		worlds.Add( new WorldID(2,"the_forest","the_forest_desc",20,UIMenu[1],1,World2Assets, World2Backgrounds));

		worlds.Sort();

		foreach(WorldID world in worlds)
		{
			Debug.LogFormat ("Mundo {0} ({1}) creada.", world.ID, world.name);
		}

		//This clears out the list so that it is empty.
		//mission.Clear();
	}
	// Use this for initialization
	void Start () {
		CreateWorlds ();
		LoadMissionsData ();
	}


	public void LoadMissionsData()

	{
		foreach(WorldID world in worlds)
		{
			GameObject worldPrefab = (GameObject)Instantiate (worldsPrefab,transform.position,  Quaternion.identity);
			worldPrefab.transform.SetParent(worldList.transform);
			//Crear gameobject
			worldPrefab.name = world.ID.ToString();
			worldPrefab.GetComponent<Image>().sprite = UIMenuBtn[world.UIMenuB];
			worldPrefab.GetComponent<WorldButton> ().worldID = world.ID;
			worldPrefab.transform.GetChild (0).GetComponent<Text> ().text = world.name;
			worldPrefab.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);

		}
			
	}
}


public class WorldID : IComparable<WorldID>{

	public int ID;
	public string name;
    public string desc;
	public int num_missions;
	public GameObject UIMenu;
	public int UIMenuB;
	public List<GameObject> GameAssets = new List<GameObject>();
    public List<GameObject> GameBackgrounds = new List<GameObject>();

    public WorldID(int newID, string newName, string newDesc, int newNum_M, GameObject newUIM, int newUIB, List<GameObject> newGAssets, List<GameObject> newGBackgrounds)
	{
		ID = newID;
		name = newName;
        desc = newDesc;
		num_missions = newNum_M;
		UIMenu = newUIM;
		UIMenuB = newUIB;
		GameAssets = newGAssets;
        GameBackgrounds = newGBackgrounds;
    }

	public int CompareTo(WorldID other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return
		return ID - other.ID;
	}

}

