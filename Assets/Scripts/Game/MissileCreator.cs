using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCreator : MonoBehaviour {

	private float screenWidthInPoints;
	public Indicator arrowIndicator;
    public GameObject target;
	public GameObject[] availableObjects;    
	public List<GameObject> objects;

	public float objectsMinDistance = 5.0f;    
	public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
	public float objectsMaxY = 1.4f;

	public float objectsMinRotation = -45.0f;
	public float objectsMaxRotation = 45.0f;

	void Start () {
		//The screen size will be used to determine if you need to generate new room

		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
    }

	void AddObject(float lastObjectX)
	{
		//Generates a random index for the object to generate. This can be a laser or one of the coin packs.
		int randomIndex = Random.Range(0, availableObjects.Length);

		//Creates an instance of the object that was just randomly selected.
		GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

		Indicator arrow = (Indicator)Instantiate(arrowIndicator, new Vector2(0, 0), Quaternion.identity);
		arrow.gameObject.transform.localScale = new Vector2 (0, 0);
		arrow.target = obj;

        //Sets the object’s position, using a random interval and a random height. This is controlled by script 
        //parameters.
        objectsMinY = target.transform.position.y;
        objectsMaxY = target.transform.position.y + 0.4f;

        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(objectsMinY, objectsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 

		//Adds a random rotation to the newly placed objects.
		float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
		obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

		//Adds the newly created object to the objects list for tracking and ultimately,
		//removal (when it leaves the screen).
		objects.Add(obj);            
	}

	void GenerateObjectsIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeObjectsX = playerX - screenWidthInPoints;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;

		//2
		List<GameObject> objectsToRemove = new List<GameObject>();

		foreach (var obj in objects)	
		{
			if (obj) {
				//3
				float objX = obj.transform.position.x;

				//4
				farthestObjectX = Mathf.Max (farthestObjectX, objX);

				//5
				if (objX < removeObjectsX)
					objectsToRemove.Add (obj);
			} else {
				objectsToRemove.Add (obj);
			}
		}

		//6
		foreach (var obj in objectsToRemove)
		{
			if (obj) {
				Destroy (obj);
			}
			objects.Remove (obj);
		}

		//7
		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
	}


	void FixedUpdate () 
	{    
		if(GameManager.GameM.RunGame){
			GenerateObjectsIfRequired();
		}
	}
}
