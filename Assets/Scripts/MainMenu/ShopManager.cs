using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObjectType;

public class ShopManager : MonoBehaviour {

	public static ShopManager ShopM;

	public GameObject ObjectShopPrefab;
	public GameObject ObjectsList;
	public GameObject InsufficientCarrotsUI;

    private Dictionary<int, GameObject> ActualGameO = new Dictionary<int, GameObject>();

    void Awake(){
		ShopM = this;
	}
	// Use this for initialization
	void Start () {
		InsufficientCarrotsUI.SetActive (false);
		CreateObjectsShop ();
	}
	
	// Update is called once per frame
	public void CreateObjectsShop(int category = 1) {
        for (int x = 0; x < ObjectsList.transform.childCount; x++)
            Destroy(ObjectsList.transform.GetChild(x).gameObject);

        ActualGameO.Clear();

        if (category == 1)
        {
            foreach (ObjectID object_shop in ObjectsManager.ObjectsM.objects)
            {
                if (object_shop.in_shop == 1)
                {
                    GameObject objectPrefab = (GameObject)Instantiate(ObjectShopPrefab, transform.position, Quaternion.identity);
                    objectPrefab.transform.SetParent(ObjectsList.transform);
                    objectPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    objectPrefab.name = object_shop.ID.ToString();

                    ObjectsManager.ObjectsM.InstantiateObject(object_shop.image_index + 1, objectPrefab.transform.GetChild(0).gameObject);

                    objectPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ObjectsManager.ObjectsM.GetNameObject(object_shop.ID);
                    objectPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ObjectsManager.ObjectsM.GetDescObject(object_shop.ID);

                    objectPrefab.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = object_shop.cost_shop.ToString();
                    objectPrefab.transform.GetChild(5).GetComponent<ObjectsButton>().typeO = OType.Boost;
                    objectPrefab.transform.GetChild(5).GetComponent<ObjectsButton>().ObjectID = object_shop.ID;
                    ActualGameO.Add(object_shop.ID, objectPrefab);
                    UpdateObjectQ(object_shop.ID, OType.Boost);

                }
            }
        }
        else if (category == 2) {
            foreach (ObjectID object_shop in ObjectsManager.ObjectsM.vehicles)
            {
                if (object_shop.in_shop == 1)
                {
                    GameObject objectPrefab = (GameObject)Instantiate(ObjectShopPrefab, transform.position, Quaternion.identity);
                    objectPrefab.transform.SetParent(ObjectsList.transform);
                    objectPrefab.transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
                    objectPrefab.name = object_shop.ID.ToString();

                    ObjectsManager.ObjectsM.InstantiateVehicle(object_shop.image_index + 1, objectPrefab.transform.GetChild(0).gameObject);

                    objectPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ObjectsManager.ObjectsM.GetNameVehicle(object_shop.ID);
                    objectPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ObjectsManager.ObjectsM.GetDescVehicle(object_shop.ID);
                    objectPrefab.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = object_shop.cost_shop.ToString();
                    objectPrefab.transform.GetChild(5).GetComponent<ObjectsButton>().typeO = OType.Vehicle;
                    objectPrefab.transform.GetChild(5).GetComponent<ObjectsButton>().ObjectID = object_shop.ID;
                    ActualGameO.Add(object_shop.ID, objectPrefab);
                    UpdateObjectQ(object_shop.ID, OType.Vehicle);
                }
            }
        }
	}


	public void BuyObject(int ID_object, OType type)
    {
        if (ObjectsManager.ObjectsM.GetCostObject(ID_object, type) > UserInfo.UserI.carrots) {
			InsufficientCarrotsUI.SetActive (true);
			Debug.Log("Zanahorias insuficientes");
		} else {
            int inventory = 0;
            if (UserInfo.UserI.UserHasObject(ID_object, type))
            {
                inventory = UserInfo.UserI.CountObject(ID_object, type);
            }
            if (inventory < ObjectsManager.ObjectsM.GetLimitObject(ID_object, type) || ObjectsManager.ObjectsM.GetLimitObject(ID_object, type) == -1)
            {
                RewardSystem.RewardS.CalculateObjects(ID_object, 1, type);
                RewardSystem.RewardS.CalculateCarrots(-ObjectsManager.ObjectsM.GetCostObject(ID_object, type));
                UpdateObjectQ(ID_object, type);
                Debug.Log("Objeto comprado.");
            }
            else {
                Debug.Log("Objeto limite alcanzado.");
            }

		}
		
	}

    private void UpdateObjectQ(int ID_object, OType type)
    {
        
        int inventory = 0;
        Dictionary<string, int> objects = new Dictionary<string, int>();
        if (type == OType.Boost)
            objects = UserInfo.UserI.objects;
        else if (type == OType.Vehicle)
            objects = UserInfo.UserI.vehicles;
        Debug.Log("Actualizando");
        if (UserInfo.UserI.UserHasObject(ID_object,type))
        {
            inventory = UserInfo.UserI.CountObject(ID_object, type);
        }
        

        if (GetObjectPrefabId(ID_object) != null)
        {   if(inventory == 0)
            {
                GetObjectPrefabId(ID_object).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "";
            }
            else if (inventory < ObjectsManager.ObjectsM.GetLimitObject(ID_object, type) || ObjectsManager.ObjectsM.GetLimitObject(ID_object, type) == -1)
            {
                GetObjectPrefabId(ID_object).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("%you_have%"), "<color=#F2A500FF>"+ inventory + "</color>");
            }
            else
            {
                GetObjectPrefabId(ID_object).transform.GetChild(5).GetComponent<Button>().interactable = false;
                GetObjectPrefabId(ID_object).transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format(Lean.Localization.LeanLocalization.GetTranslationText("%you_have_limit%"), "<color=#F2A500FF>" + inventory + "</color>");
                
            }
        }
        else { Debug.Log("NO ENCONTRADO"); }

    }

    public GameObject GetObjectPrefabId(int id)
    {
        if (ActualGameO.ContainsKey(id))
        {
            return ActualGameO[id];
        }
        Debug.LogWarningFormat("Object  ID: {0} doesn't exist!", id);
        return null;
    }

}
