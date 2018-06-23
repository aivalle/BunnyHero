using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsButton : MonoBehaviour {
	
	public Sprite BlankSelection;
	public int indexButton;
 
	public void Pushed(){
		ShopManager.ShopM.BuyObject(int.Parse(gameObject.transform.parent.name));
	}

	public void SelectObject(){
		if(GameManager.GameM.objects_game.Contains(int.Parse (gameObject.name))){
			GameManager.GameM.PanelLargeUI.SetActive (false);
		}else{
			GameManager.GameM.PanelObjects.transform.GetChild(GameManager.GameM.indexButton).GetComponent<Image> ().sprite = gameObject.GetComponent<Image> ().sprite;
			if (int.Parse (gameObject.name) != 0) {
				GameManager.GameM.objects_game.Remove (int.Parse (GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).name ));
			}
			GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).name = gameObject.name;
			GameManager.GameM.objects_game.Add (int.Parse (gameObject.name));
			GameManager.GameM.PanelLargeUI.SetActive (false);
			GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	public void OpenSelection(){
		GameManager.GameM.PanelLargeUI.SetActive (true);
		GameManager.GameM.indexButton = indexButton;

		GameManager.GameM.idObjRemove = int.Parse (gameObject.name);
	
	}

	public void DeleteSelection(){
		GameManager.GameM.indexButton = indexButton;
		GameManager.GameM.objects_game.Remove (int.Parse (GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).name ));
		GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).GetComponent<Image> ().sprite = BlankSelection;
		GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).transform.GetChild (0).gameObject.SetActive (false);
		GameManager.GameM.PanelObjects.transform.GetChild (GameManager.GameM.indexButton).name = "0";
	}

	public void ActivateObject(){
		GameManager.GameM.UseObject (int.Parse (gameObject.name), gameObject.GetComponent<Button> ());
	}
		

}