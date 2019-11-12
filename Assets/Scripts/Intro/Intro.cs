using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Intro : MonoBehaviour {

	// Use this for initialization

    private void Start()
    {
       
        LevelManager.LevelM.LoadScene("Menu");
    }
}
