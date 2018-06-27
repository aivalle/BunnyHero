using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MissionID : IComparable<MissionID>{

	public int ID;
	public int worldID;
	public string desc;
	public int time;
	public int hits;
	public int distance;
	public int rewardEXP;
	public Dictionary <int,int> rewards = new Dictionary <int,int>();
	public int hitMode;
	public int objectsAvailable;
	public int final_reward;

	public MissionID(int newID, int newWorldID, string newDesc, int newTime, int newHits,int newDistance,int newRewardEXP, Dictionary <int,int> newRewards,  int newHitMode, int newObjectsAvailable,int newFinal_reward)
	{
		ID = newID;
		worldID = newWorldID;
		desc = newDesc;
		time = newTime;
		hits = newHits;
		distance = newDistance;
		rewardEXP = newRewardEXP;
		rewards = newRewards;
		hitMode = newHitMode;
		objectsAvailable = newObjectsAvailable;
		final_reward = newFinal_reward;
	}
		
	public int CompareTo(MissionID other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return
		return ID - other.ID;
	}

}
