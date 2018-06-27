using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSprite : MonoBehaviour {

	Animation animSprite;
	public Image sprite;
	public Sprite DefaultImg;
	public Sprite CheckedImg;
	public bool isChecked;
	public List<Sprite> listSprites = new List<Sprite>();

	// Use this for initialization
	public void Start() {
		animSprite = GetComponent<Animation> ();
		StartCoroutine ("GallerySprite");
	}

	public IEnumerator GallerySprite(){
		if (!isChecked) {
			int index = 0; 
			int countSprites = listSprites.Count;
			while (index < listSprites.Count) {
				sprite.sprite = listSprites [index];
				animSprite.Play ("ZoomIn");
				yield return new WaitForSeconds (1.5f);
				index++;
			}
			if (countSprites > 1) {
				StartCoroutine ("GallerySprite");
			} else if (countSprites == 0) {
				sprite.sprite = DefaultImg;
			}
		} else {
			sprite.sprite = CheckedImg;
		}
	}

}
