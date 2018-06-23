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
				RewardSystem.RewardS.FirstReward ();
				SaveInfo ();
			}
		} else {
			RewardSystem.RewardS.FirstReward ();
			SaveInfo ();
		}
	}

	void Awake(){
		serializer = this;
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


	public static Dictionary<string, List<int>> ObjectToDictionaryAdvance(object obj)
	{
		if (typeof(IDictionary).IsAssignableFrom(obj.GetType()))
		{
			IDictionary idict = (IDictionary)obj;

			Dictionary<string, List<int>> newDict = new Dictionary<string, List<int>>();
			foreach (string key in idict.Keys)
			{
				//Obtener la lista de valores en key por bucle:
				IList copy = (IList) idict[key.ToString()];

				List<int> newL = new List<int> ();
				for (int i = 0; i < copy.Count; i++)
				{
					newL.Add (int.Parse(copy [i].ToString()));
				}
				newDict.Add(key.ToString(), newL);

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
	public Dictionary<string,List<int>> missionsComplete = new Dictionary<string, List<int>>();
	public string LastFuelTimer;
	public Dictionary<string,int> objects = new Dictionary<string,int>();
}
