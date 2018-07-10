using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Globalization;
using System.Collections.Generic;
using MiniJSON;
using System.Linq;


public class Serializer : MonoBehaviour
{

	public static Serializer serializer;

    static readonly string SAVE_FILE = "player.dat";
                                               //01234567890123456789012345678901
    static readonly string JSON_ENCRYPTED_KEY = "#kJ83DAlowjkf39(#($%0_+[]:#dDA'a";

	void Awake(){
		serializer = this;
	}

	public void LoadInfo(){

		string filename = Path.Combine(Application.persistentDataPath, SAVE_FILE);

		if (File.Exists (filename)) {
			try{
				Rijndael crypto = new Rijndael ();

				byte[] soupBackIn = File.ReadAllBytes (filename);
				string jsonFromFile = crypto.Decrypt (soupBackIn, JSON_ENCRYPTED_KEY);
				
			Debug.Log (jsonFromFile);

			var copy = Json.Deserialize(jsonFromFile) as Dictionary<string, object>;

			UserInfo.UserI.objects = ObjectToDictionary(copy ["objects"]);
			UserInfo.UserI.exp =  int.Parse(copy["exp"].ToString());
			UserInfo.UserI.carrots =  int.Parse(copy["carrots"].ToString());
			UserInfo.UserI.missionsComplete = ObjectToDictionaryAdvance(copy["missionsComplete"]);
			UserInfo.UserI.fuelGame =  int.Parse(copy["fuelGame"].ToString());
			UserInfo.UserI.LastFuelTimer = DateTime.ParseExact(copy["LastFuelTimer"].ToString(),"d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

			UserInfo.UserI.objects = ObjectToDictionary(copy ["objects"]);

			Debug.Log ("Datos obtenidos");
			}catch(Exception ex) {
				FirstReward ();
				SaveInfo ();
			}
		} else {
			FirstReward ();
			SaveInfo ();
		}
	}

	public void FirstReward(){
		UserInfo.UserI.carrots = 50;
		UserInfo.UserI.fuelGame = 3;
		UserInfo.UserI.exp = 0;
		UserInfo.UserI.LastFuelTimer = DateTime.UtcNow;
	}

	public void SaveInfo(){

		string format = "d/M/yyyy HH:mm:ss";
		Dictionary<string,object> dict = new Dictionary<string,object> ();

		dict.Add ("carrots", UserInfo.UserI.carrots);
		dict.Add ("exp", UserInfo.UserI.exp);
		dict.Add ("missionsComplete", UserInfo.UserI.missionsComplete);
		dict.Add ("fuelGame", UserInfo.UserI.fuelGame);
		dict.Add ("LastFuelTimer", UserInfo.UserI.LastFuelTimer.ToString(format));
		dict.Add ("objects", UserInfo.UserI.objects);

		string json = Json.Serialize(dict);

		Debug.Log (json);
		Rijndael crypto = new Rijndael();
		byte[] soup = crypto.Encrypt(json, JSON_ENCRYPTED_KEY);

		string filename = Path.Combine(Application.persistentDataPath, SAVE_FILE);

		if (File.Exists(filename))
		{
			File.Delete(filename);
		}

		File.WriteAllBytes(filename, soup);

		Debug.Log("Datos guardados");

	}
    void Start()
    {
		LoadInfo ();

    }
		

	void OnApplicationQuit(){

		Serializer.serializer.SaveInfo ();
	}


	public static Dictionary<string, int> ObjectToDictionary(object obj)
	{
		if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
		{
			IDictionary idict = (IDictionary)obj;

			Dictionary<string, int> newDict = new Dictionary<string, int>();
			foreach (object key in idict.Keys)
			{
				newDict.Add(key.ToString(), int.Parse(idict[key].ToString()));
			}
			return newDict;
		}
		else
		{
			Debug.LogError ("Object is not dictionary.");
			return null;
		}
	}


	public static Dictionary<int, Dictionary<int,Dictionary<string,object>>> ObjectToDictionaryAdvance(object obj)
	{
		if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
		{
			IDictionary idict = (IDictionary)obj;

			Dictionary<int, Dictionary<int,Dictionary<string,object>>> newDict = new Dictionary<int, Dictionary<int,Dictionary<string,object>>>();

			foreach (string key in idict.Keys)//Recorre mundos
			{
				IDictionary copy =  (IDictionary)idict[key];

				Dictionary<int,Dictionary<string,object>> newL = new Dictionary<int,Dictionary<string,object>>();

				foreach (string key2 in copy.Keys) //Recorre ID
				{
					Dictionary<string,object> newL2 = new Dictionary<string,object> ();
					IDictionary copy2 = (IDictionary) copy[key2]; ////////////

					foreach (string key3 in copy2.Keys)//Recorrer valores como keys
					{
						newL2.Add (key3, copy2[key3]);
					}

					newL.Add (Int32.Parse(key2), newL2);
				}

				newDict.Add(Int32.Parse(key), newL);

			}

			return newDict;
		}
		else
		{
			Debug.LogError ("Object is not dictionary.");
			return null;
		}
	}

}
	

[Serializable]
public class SaveData
{
	public int carrots;
	public int fuelGame;
	public int exp;
	public Dictionary<int, Dictionary<int,Dictionary<string,object>>> missionsComplete = new Dictionary<int, Dictionary<int,Dictionary<string,object>>>();
	public string LastFuelTimer;
	public Dictionary<string,int> objects = new Dictionary<string,int>();
}
