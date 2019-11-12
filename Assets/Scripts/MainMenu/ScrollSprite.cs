using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSprite : MonoBehaviour {

	Animation animSprite;
	public GameObject objectView;
	public GameObject DefaultImg;
	public GameObject CheckedImg;
	public bool isChecked = false;
	public List<GameObject> listSprites = new List<GameObject>();
    public float timeChange = 1.0f;
    private float nextChange;

    private int index;
    private int countSprites = 0;
    private bool enabledScroll = false;

    // Use this for initialization
    public void Start() {
        animSprite = GetComponent<Animation>();
        countSprites = listSprites.Count;
        for (int i = 0; i < objectView.transform.childCount; i++)
            objectView.transform.GetChild(i).gameObject.SetActive(false);
        bool enabled = true;
        if (isChecked)
        {
            ClearAllSprites();
            GameObject objectPrefab = (GameObject)Instantiate(CheckedImg, transform.position, Quaternion.identity);
            objectPrefab.transform.SetParent(objectView.transform, false);
            objectPrefab.SetActive(true);
            enabled = false;
        }
        else if (countSprites == 0)
        {
            ClearAllSprites(false);
            GameObject objectPrefab = (GameObject)Instantiate(DefaultImg, transform.position, Quaternion.identity);
            objectPrefab.transform.SetParent(objectView.transform, false);
            objectPrefab.SetActive(true);
            enabled = false;
        }
        else if (countSprites > 0) {
            listSprites[0].SetActive(true);
            if (countSprites == 1)
                enabled = false;
        }
        countSprites = listSprites.Count;
        index = 0;
        enabledScroll = enabled;
    }

    public void ClearAllSprites(bool list= true) {
        for (int i = 0; i < objectView.transform.childCount; i++)
            Destroy(objectView.transform.GetChild(i).gameObject);
        if(list)
            listSprites.Clear();
    }

    void Update()
    {

        if (Time.time > nextChange && enabledScroll)
        {
            if (index < countSprites)
            {
                if(index > 0)
                    listSprites[index-1].SetActive(false);
                else
                    listSprites[countSprites-1].SetActive(false);

                listSprites[index].SetActive(true);
                animSprite.Play("ZoomIn");
                index++;
            }
            else {
                index = 0;
            }
            nextChange = Time.time + timeChange;
        }
    }
}
