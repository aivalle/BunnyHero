using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour {

	public static ShopManager ShopM;

	public GameObject ObjectShopPrefab;
	public GameObject ObjectsList;
	public GameObject InsufficientCarrotsUI;

	void Awake(){
		ShopM = this;
	}
	// Use this for initialization
	void Start () {
		InsufficientCarrotsUI.SetActive (false);
		CreateObjectsShop ();
	}
	
	// Update is called once per frame
	public void CreateObjectsShop() {
		int i = 0;
		foreach (ObjectID object_shop in ObjectsManager.ObjectsM.objects) {
			if (object_shop.in_shop == 1) {
				GameObject objectPrefab = (GameObject)Instantiate (ObjectShopPrefab, transform.position, Quaternion.identity);
				objectPrefab.transform.SetParent (ObjectsList.transform);
				objectPrefab.transform.localScale = new Vector3 (1.0f, 1.0f, 0.0f);
				objectPrefab.name = object_shop.ID.ToString();
				objectPrefab.transform.GetChild(0).GetComponent<Image> ().sprite = ObjectsManager.ObjectsM.images_objects [object_shop.image_index];
				objectPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI> ().text = ObjectsManager.ObjectsM.GetNameObject(object_shop.ID);
				objectPrefab.transform.GetChild (2).GetComponent<TextMeshProUGUI> ().text = ObjectsManager.ObjectsM.GetDescObject (object_shop.ID);
				int inventory = 0;
				if (UserInfo.UserI.objects.ContainsKey (object_shop.ID.ToString())) {
					inventory = UserInfo.UserI.objects [object_shop.ID.ToString()];
				}


				objectPrefab.transform.GetChild(3).GetComponent<TextMeshProUGUI> ().text = string.Format ("Tienes <color=#F2A500FF>{0}</color>.", inventory);

				objectPrefab.transform.GetChild(4).GetComponent<TextMeshProUGUI> ().text = object_shop.cost_shop.ToString();
				i++;
			}
		}
	}


	public void BuyObject(int ID_object){

		int objectarray = ID_object - 1;

		int cost = ObjectsManager.ObjectsM.objects [objectarray].cost_shop;
		if (cost > UserInfo.UserI.carrots) {
			//Carrots insuficientes
			InsufficientCarrotsUI.SetActive (true);
			Debug.Log("Zanahorias insuficientes");
		} else {

			RewardSystem.RewardS.CalculateObjects (1, ID_object);
			RewardSystem.RewardS.CalculateCarrots (-cost);
			Debug.Log ("Objeto comprado.");
		}
		
	}

}
