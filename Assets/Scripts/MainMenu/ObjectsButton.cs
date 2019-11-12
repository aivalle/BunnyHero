using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectType;

public class ObjectsButton : MonoBehaviour {
	
	public Sprite BlankSelection;
	public int indexButton;
    public int ObjectID = 0;
    public enum TypeObject {Boost, Vehicle}

    public OType typeO;

    public void Pushed(){

        ShopManager.ShopM.BuyObject(GetComponent<ObjectsButton>().ObjectID, typeO);
        
    }

	public void SelectObject(){
		if(MissionsManager.MissionsM.objects_game.Contains(int.Parse (gameObject.name))){
		}else{
            if (int.Parse(gameObject.name) != 0)
            {
                MissionsManager.MissionsM.PanelUIObjects.SetActive(true);
                ObjectsManager.ObjectsM.InstantiateObject(int.Parse(gameObject.name), MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).gameObject);
                //MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).GetComponent<Image> ().sprite = gameObject.GetComponent<Image> ().sprite;
               
                MissionsManager.MissionsM.objects_game.Remove(int.Parse(MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).name));

                MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).name = gameObject.name;
                MissionsManager.MissionsM.objects_game.Add(int.Parse(gameObject.name));
                MissionsManager.MissionsM.PanelUIObjects.SetActive(false);

                MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).transform.GetChild(0).transform.SetSiblingIndex(1);
                MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).transform.GetChild(1).gameObject.SetActive(true);
            }
		}
	}

	public void OpenSelection(){
        MissionsManager.MissionsM.PanelUIObjects.SetActive(true);
        MissionsManager.MissionsM.indexButton = indexButton;

        MissionsManager.MissionsM.idObjRemove = ObjectID;
	
	}

	public void DeleteSelection(){
        MissionsManager.MissionsM.indexButton = indexButton;
        MissionsManager.MissionsM.objects_game.Remove (int.Parse (MissionsManager.MissionsM.PanelObjects.transform.GetChild (MissionsManager.MissionsM.indexButton).name ));
        Destroy(MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).transform.GetChild(0).gameObject);
        MissionsManager.MissionsM.PanelObjects.transform.GetChild (MissionsManager.MissionsM.indexButton).transform.GetChild (1).gameObject.SetActive (false);
        MissionsManager.MissionsM.PanelObjects.transform.GetChild(MissionsManager.MissionsM.indexButton).GetComponent<ObjectsButton>().ObjectID = 0;
        MissionsManager.MissionsM.PanelObjects.transform.GetChild (MissionsManager.MissionsM.indexButton).name = "0";
	}

	public void ActivateObject(){
		ObjectsManager.ObjectsM.UseObject (int.Parse (gameObject.name), gameObject.GetComponent<Button> ());
	}
		

}