using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour {
	//Objects variables.
	
	public GameObject[] availableClouds;    
	public List<GameObject> clouds;
	
	public float cloudsMinDistance = 5.0f;    
	public float objectsMaxDistance = 10.0f;
	
	public float cloudsMinY = -1.4f;
	public float cloudsMaxY = 1.4f;


	private float screenWidthInPoints = 15f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddObject(float lastObjectX)
	{
		//Generates a random index for the object to generate. This can be a laser or one of the coin packs.
		int randomIndex = Random.Range(0, availableClouds.Length);
		
		//Creates an instance of the object that was just randomly selected.
		GameObject obj = (GameObject)Instantiate(availableClouds[randomIndex]);
		
		//Sets the object’s position, using a random interval and a random height. This is controlled by script 
		//parameters.
		float objectPositionX = lastObjectX + Random.Range(cloudsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(cloudsMinY, cloudsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 
		
		//Adds the newly created object to the objects list for tracking and ultimately,
		//removal (when it leaves the screen).
		clouds.Add(obj);            
	}

	void GenerateObjectsIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeObjectsX = playerX - screenWidthInPoints - 10f;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;
		
		//2
		List<GameObject> objectsToRemove = new List<GameObject>();
		
		foreach (var obj in clouds)
		{
			//3
			float objX = obj.transform.position.x;
			
			//4
			farthestObjectX = Mathf.Max(farthestObjectX, objX);
			
			//5
			if (objX < removeObjectsX)            
				objectsToRemove.Add(obj);
		}
		
		//6
		foreach (var obj in objectsToRemove)
		{
		clouds.Remove(obj);
			Destroy(obj);
		}
		
		//7
		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
	}
	
	void FixedUpdate () 
	{    
		GenerateObjectsIfRequired();
	}
}

