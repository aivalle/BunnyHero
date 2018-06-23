using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectsManager : MonoBehaviour {


	public static ObjectsManager ObjectsM;

	public List<ObjectID> objects = new List<ObjectID>();

	public List<Sprite> images_objects; 
	public List<string> names_objects; 
	public List<string> desc_objects; 

	void Awake () {
		ObjectsM = this;
	}

	// Use this for initialization
	void Start () {
		objects.Add( new ObjectID(1,10,0,0,0,1));
		objects.Add( new ObjectID(2,20,1,1,1,1));
		objects.Add( new ObjectID(3,15,2,2,2,1));

	}

	public string GetDescObject(int ID){
		return desc_objects [objects[ID-1].desc_index];
	}

	public string GetNameObject(int ID){
		return names_objects [objects[ID-1].name_index];
	}

	public Sprite GetImageObject(int ID){
		return images_objects [objects[ID-1].image_index];
	}


}
