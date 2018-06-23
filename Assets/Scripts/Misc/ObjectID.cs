using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectID : IComparable<ObjectID>{

	public int ID;
	public int cost_shop;
	public int image_index;
	public int in_shop;
	public int name_index;
	public int desc_index;

	public ObjectID(int newID, int newCost, int newImage, int newName,int newDesc,int newIn_shop)
	{
		ID = newID;
		cost_shop = newCost;
		image_index = newImage;
		name_index = newName;
		desc_index = newDesc;
		in_shop = newIn_shop;
	}

	public int CompareTo(ObjectID other)
	{
		if(other == null)
		{
			return 1;
		}

		//Return
		return ID - other.cost_shop - other.image_index - other.in_shop;
	}
}
