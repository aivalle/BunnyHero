using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using ObjectType;

public class ObjectsManager : MonoBehaviour {


	public static ObjectsManager ObjectsM;

	public List<ObjectID> objects = new List<ObjectID>();

	public List<ObjectID> vehicles = new List<ObjectID>();

	public List<GameObject> images_objects;
    public List<GameObject> images_vehicles;

    public List<GameObject> images_vehicles_inGame;

    public GameObject DefaultCarrots;

    void Awake () {
		ObjectsM = this;
	}

	// Use this for initialization
	void Start () {
		objects.Add( new ObjectID(1,10,0,"object_1","desc_object_1",1,-1));
		objects.Add( new ObjectID(2,20,1,"object_2","desc_object_2",1,-1));
		objects.Add( new ObjectID(3,15,2,"object_3","desc_object_3",1,-1));

        vehicles.Add(new ObjectID(1, 100, 0, "vehicle_1", "desc_vehicle_1", 1, 1));
        vehicles.Add(new ObjectID(2, 200, 1, "vehicle_2", "desc_vehicle_2", 1, 1));

    }

	public string GetDescObject(int ID, bool vehicle = false){
		return objects[ID-1].desc_text;
	}

	public string GetNameObject(int ID){
		return objects[ID-1].name_text;
	}

	public GameObject GetImageObject(int ID){
        return images_objects [objects[ID-1].image_index];

	}

    public string GetDescVehicle(int ID, bool vehicle = false)
    {
        return vehicles[ID - 1].desc_text;
    }


    public string GetNameVehicle(int ID)
    {
        return vehicles[ID - 1].name_text;
    }

    public GameObject GetImageVehicle(int ID)
    {
        return images_vehicles[vehicles[ID - 1].image_index];

    }

    public GameObject GetImageVehicleInGame(int ID)
    {
        return images_vehicles_inGame[vehicles[ID - 1].image_index];

    }

    public GameObject InstantiateObject(int ID, GameObject parent, int q = 0) {
        GameObject objectPrefab = Instantiate(GetImageObject(ID), transform.position, Quaternion.identity);
        objectPrefab.transform.SetParent(parent.transform, false);
        if (q > 0)
        {
            objectPrefab.transform.GetChild(0).gameObject.SetActive(true);
            objectPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + q.ToString();
        }
        else{ 
            objectPrefab.transform.GetChild(0).gameObject.SetActive(false);
            objectPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
        return objectPrefab;
    }

    public GameObject InstantiateVehicle(int ID, GameObject parent)
    {
        GameObject objectPrefab = Instantiate(GetImageVehicle(ID), transform.position, Quaternion.identity);
        objectPrefab.transform.SetParent(parent.transform, false);
        return objectPrefab;
    }

    public GameObject InstantiateVehicleInGame(int ID, GameObject parent)
    {
        GameObject objectPrefab = Instantiate(GetImageVehicleInGame(ID), transform.position, Quaternion.identity);
        objectPrefab.transform.SetParent(parent.transform, false);
        return objectPrefab;
    }

    public GameObject InstantiateCarrotObject(GameObject parent, int q = 0)
    {
        GameObject objectPrefab = Instantiate(DefaultCarrots, transform.position, Quaternion.identity);
        objectPrefab.transform.SetParent(parent.transform, false);
        if (q > 0)
        {
            objectPrefab.transform.GetChild(0).gameObject.SetActive(true);
            objectPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + q.ToString();
        }
        else
        {
            objectPrefab.transform.GetChild(0).gameObject.SetActive(false);
            objectPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
        }
        return objectPrefab;
    }

    public int GetCostObject(int ID, OType type)
    {
        int cost = 0;
        if (type == OType.Boost)
            cost = ObjectsManager.ObjectsM.objects[ID - 1].cost_shop;
        else if (type == OType.Vehicle)
            cost = ObjectsManager.ObjectsM.vehicles[ID - 1].cost_shop;
        return cost;
    }

    public int GetLimitObject(int ID, OType type)
    {
        int limit = 0;
        if (type == OType.Boost)
            limit = ObjectsManager.ObjectsM.objects[ID - 1].limit;
        else if (type == OType.Vehicle)
            limit = ObjectsManager.ObjectsM.vehicles[ID - 1].limit;
        return limit;
    }

    // Sistema que permite el uso de los objetos en el juego.
    public void UseObject(int ID_Object, Button button_gO = null){
		if (button_gO != null) {
			button_gO.interactable = false;
		}
		GameManager.GameM.CurrentObject.Add(ID_Object);
		if (ID_Object == 1) { //Shield bunny
			StartCoroutine (activeShieldBunny (button_gO));
		} else if (ID_Object == 2) {
			StartCoroutine (activeMagnet (button_gO));
		} else if (ID_Object == 3) {
			StartCoroutine (activeExtraVelocity(button_gO));
		}
        RewardSystem.RewardS.CalculateObjects(ID_Object, -1, OType.Boost);
    }

	//OBJETO: Shield
	public IEnumerator activeShieldBunny(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		GameManager.GameM.CircleColl2D.enabled = false;
		BunnyManager.BunnyM.shieldBunny.SetActive (true);
		float newtime = 5;
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		GameManager.GameM.CircleColl2D.enabled = true;
		BunnyManager.BunnyM.shieldBunny.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(1);
		if (button != null) {
			button.interactable = true;
		}
	}

	//OBJETO: Extra velocity
	public IEnumerator activeExtraVelocity(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		int adds = 0;
		float newtime = 10;
		while (adds < 3) {
			BunnyManager.BunnyM.rb.AddForce (new Vector2 (1500, 280));
			adds++;
		}
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(1);
		if (button != null) {
			button.interactable = true;
		}
	}

	//OBJETO: Magnet
	public IEnumerator activeMagnet(Button button = null){
		GameObject btn = button.gameObject.transform.GetChild (0).gameObject;
		btn.SetActive (true);
		BunnyManager.BunnyM.MagnetEffect.SetActive (true);
		float newtime = 20;
		btn.GetComponent<Text> ().text = newtime.ToString ();
		while (newtime > 0f) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime--;
				btn.GetComponent<Text> ().text = newtime.ToString ();
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		btn.SetActive (false);
		BunnyManager.BunnyM.MagnetEffect.SetActive (false);
		GameManager.GameM.CurrentObject.Remove(2);
		if (button != null) {
			button.interactable = true;
		}
	}

	public IEnumerator GenericObject(float duration, int Object_ID, Button button = null){
		float newtime = 0;
		while (newtime < duration) {
			if (GameManager.GameM.GamePause == false) {
				yield return new WaitForSeconds (1f);
				newtime++;
			} else {
				yield return new WaitForSeconds (1f);
			}
		}
		GameManager.GameM.CurrentObject.Remove (Object_ID);
		if (button != null) {
			button.interactable = true;
		}
	}


}

namespace ObjectType
{
    public enum OType
    {
        Boost,
        Vehicle,
    }
}
