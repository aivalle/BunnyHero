using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectSnap : MonoBehaviour {

	//public static ScrollRectSnap ScrollRectS;
	public RectTransform panel;
	public GameObject[] bttn;
	public RectTransform center;
	public bool work;
	public bool stop;

	public float[] distance;
	private bool dragging = false;
	public int bttnDistance;
	public int minButtnNum;

	void Awake () {
		//ScrollRectS = this;
	}


	public void WorkNow(){
		work = true;
		int bttnLenght = bttn.Length;
		distance = new float[bttnLenght];
		//Get distance between buttons
		bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

	}

	void Update(){
		if(work == true){
			bttnDistance = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);

				//Define distances between center and all buttons
				for (int i = 0; i < bttn.Length; i++) {
					distance [i] = Mathf.Abs (center.transform.position.x - bttn [i].transform.position.x);	
				}
				//Define which button is in center
				float minDistance = Mathf.Min (distance);
			if (stop == false) {	
				for (int a = 0; a < bttn.Length; a++) {
					if (minDistance == distance [a]) {
						minButtnNum = a;
					}
				}
			}
				//If the user isn't dragging, center the most near button
				if (!dragging) {
					LerpToBttn (minButtnNum * -bttnDistance);
				}
			
		}
	}

	void LerpToBttn(int position){
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 10f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);
		panel.anchoredPosition = newPosition;
	} 

	public void StartDrag(){
		dragging = true;
	}

	public void EndDrag(){
		dragging = false;
	}

	public void Gotobutton(int index){
		StartCoroutine( GoToPosition(index));
	}

	IEnumerator GoToPosition(int index){
		stop = true;
		minButtnNum = index;
		yield return new WaitForSeconds(1f);
		stop = false;
	}
}
