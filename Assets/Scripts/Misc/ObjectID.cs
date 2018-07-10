using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectID : IComparable<ObjectID>{

	public int ID;
	public int cost_shop;
	public int image_index;
	public int in_shop;
	public string name_text;
	public string desc_text;
	public int limit;

	public ObjectID(int newID, int newCost, int newImage, string newName,string newDesc,int newIn_shop, int newLimit)
	{
		ID = newID;
		cost_shop = newCost;
		image_index = newImage;
		name_text = newName;
		desc_text = newDesc;
		in_shop = newIn_shop;
		limit = newLimit;
	}

	public int CompareTo(ObjectID other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return
		return ID;
	}
}
