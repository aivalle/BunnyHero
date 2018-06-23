using UnityEngine;
using System.Collections;
//------------------------------
using System.Collections.Generic;

public class SceneCreator: MonoBehaviour {

	//Variables Start____________________________________

	public List<GameObject> availableRooms;

	public List<GameObject> currentRooms;

	private float screenWidthInPoints;

	//Objects variables.

	public List<GameObject> availableObjects;    
	public List<GameObject> objects;

	public float objectsMinDistance = 5.0f;    
	public float objectsMaxDistance = 10.0f;

	public float objectsMinY = -1.4f;
	public float objectsMaxY = 1.4f;

	public float objectsMinRotation = -45.0f;
	public float objectsMaxRotation = 45.0f;

	//Variables End______________________________________

	// Use this for initialization
	void Start () {
		//The screen size will be used to determine if you need to generate new room

		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;

	}

	// Update is called once per frame
	void Update () {

	}

	void AddRoom(float farhtestRoomEndX)
	{
		//Picks a random index of the room type (Prefab) to generate.
		int randomRoomIndex = Random.Range(0, availableRooms.Count);

		//Creates a room object from the array of available rooms using the random index above.
		GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

		//Since the room is just an Empty containing all the room parts, you cannot simply take 
		//its size. Instead you get the size of the floor inside the room, which is equal to 
		//the room’s width.
		float roomWidth = room.transform.Find("limits").localScale.x;

		//When you set the room position, you set the position of its center so you add the 
		//half room width to the position where the level ends. This way gets the point at 
		//which you should add the room, so that it started straight after the last room.
		float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

		//This sets the position of the room. You need to change only the x-coordinate since
		//all rooms have the same y and z coordinates equal to zero.
		room.transform.position = new Vector3(roomCenter, 0, 0);

		//Finally you add the room to the list of current rooms. It will be cleared in the 
		//next method which is why you need to maintain this list.
		currentRooms.Add(room);         
	}

	void GenerateRoomIfRequired()
	{
		//Creates a new list to store rooms that needs to be removed. Separate lists are required
		//since you cannot remove items from the list while you iterating through it.
		List<GameObject> roomsToRemove = new List<GameObject>();

		//This is a flag that shows if you need to add more rooms. By default it is set to true, 
		//but most of the time it will be set to false inside the foreach.
		bool addRooms = true;        

		//Saves player position.
		float playerX = transform.position.x;

		//This is the point after which the room should be removed. If room position is behind this 
		//point (to the left), it needs to be removed.
		float removeRoomX = playerX - screenWidthInPoints;        

		//If there is no room after addRoomX point you need to add a room, since the end of the 
		//level is closer then the screen width.
		float addRoomX = playerX + screenWidthInPoints;

		//In farthestRoomEndX you store the point where the level currently ends. You will use this 
		//variable to add new room if required, since new room should start at that point to make the 
		//level seamless.
		float farthestRoomEndX = 0;

		foreach(var room in currentRooms)
		{
			//In foreach you simply enumerate current rooms. You use the floor to get the room width and 
			//calculate the roomStartX (a point where room starts, leftmost point of the room) and 
			//roomEndX (a point where the room ends, rightmost point of the room).
			float roomWidth = room.transform.Find("limits").localScale.x;
			float roomStartX = room.transform.position.x - (roomWidth * 0.5f);    
			float roomEndX = roomStartX + roomWidth;                            

			//If there is a room that starts after addRoomX then you don’t need to add rooms right now.
			//However there is no break instruction here, since you still need to check if this room 
			//needs to be removed.
			if (roomStartX > addRoomX)
				addRooms = false;

			//If room ends to the left of removeRoomX point, then it is already off the screen and needs 
			//to be removed.
			if (roomEndX < removeRoomX)
				roomsToRemove.Add(room);

			//Here you simply find the rightmost point of the level. This will be a point where the level 
			//currently ends. It is used only if you need to add a room.
			farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
		}

		//This removes rooms that are marked for removal. The mouse GameObject already flew through them 
		//and thus, they are far behind, so you need to remove them.
		foreach(var room in roomsToRemove)
		{
			currentRooms.Remove(room);
			Destroy(room);            
		}

		//If at this point addRooms is still true then the level end is near. addRooms will be true if it 
		//didn’t find a room starting farther then screen width. This indicate that a new room needs 
		//to be added.
		if (addRooms)
			AddRoom(farthestRoomEndX);
	}

	void AddObject(float lastObjectX)
	{
		//Generates a random index for the object to generate. This can be a laser or one of the coin packs.
		int randomIndex = Random.Range(0, availableObjects.Count);

		//Creates an instance of the object that was just randomly selected.
		GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

		//Sets the object’s position, using a random interval and a random height. This is controlled by script 
		//parameters.
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
			objects.Remove(obj);
			Destroy(obj);
		}

		//7
		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
	}
	void FixedUpdate () 
	{    
		GenerateRoomIfRequired();

		GenerateObjectsIfRequired();
	}
}