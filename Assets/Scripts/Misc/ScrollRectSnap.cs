using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectSnap : MonoBehaviour {

    public RectTransform ScrollPanel, CenterOfCard;
    public GameObject[] AllButtons;

    public RectTransform[] buttonRTs;
    private RectTransform centerRT;
    public float[] distances; // buttons distance to the center
    private bool isDragging = false;
    public int buttonDistance; // distance between buttons
    private int minButtonNum; // kepe track of the button closest to center
    private bool targetNearestButton = true;
    public bool work = false;

    public void WorkNow()
    {
        distances = new float[AllButtons.Length];

        buttonRTs = new RectTransform[AllButtons.Length];
        for (int i = 0; i < buttonRTs.Length; i++)
        {
            buttonRTs[i] = AllButtons[i].GetComponent<RectTransform>();
        }
        centerRT = CenterOfCard.GetComponent<RectTransform>();

        // get distance between buttons
        buttonDistance = (int)Mathf.Abs(buttonRTs[1].anchoredPosition.x - buttonRTs[0].anchoredPosition.x); // this assumes the buttons start with 0 centered
        //Debug.Log(buttonDistance);
        work = true;
    }

    private float distReposition, curX, curY;
    private Vector2 newPos;
    private void Update()
    {
        if (work)
        {
            for (int i = 0; i < AllButtons.Length; i++)
            {
                distReposition = CenterOfCard.transform.position.x - AllButtons[i].transform.position.x;
                distances[i] = Mathf.Abs(distReposition);

                if (distReposition > 1000)
                {
                    curX = buttonRTs[i].anchoredPosition.x;
                    curY = buttonRTs[i].anchoredPosition.y;
                    newPos = new Vector2(curX + (AllButtons.Length * buttonDistance), curY);
                    buttonRTs[i].anchoredPosition = newPos;
                }

                if (distReposition < -1000)
                {
                    curX = buttonRTs[i].anchoredPosition.x;
                    curY = buttonRTs[i].anchoredPosition.y;
                    newPos = new Vector2(curX - (AllButtons.Length * buttonDistance), curY);
                    buttonRTs[i].anchoredPosition = newPos;
                }
            }

            if (targetNearestButton)
            {
                float minDistance = Mathf.Min(distances);

                for (int i = 0; i < distances.Length; i++)
                {
                    if (minDistance == distances[i])
                    {
                        minButtonNum = i;
                    }
                }
            }

            if (!isDragging) // not dragging
            {
                if(minButtonNum < buttonRTs.Length)
                LerpToButton(-buttonRTs[minButtonNum].anchoredPosition.x);
                //LerpToButton(minButtonNum * -buttonDistance);
            }
        }
    }

    void LerpToButton(float position)
    {
        float newX = Mathf.Lerp(ScrollPanel.anchoredPosition.x, position, Time.deltaTime * 10f);
        Vector2 newPos = new Vector2(newX, ScrollPanel.anchoredPosition.y);
        ScrollPanel.anchoredPosition = newPos;
    }

    public void StartDrag()
    {
        isDragging = true;

        targetNearestButton = true;
    }

    public void EndDrag()
    {
        isDragging = false;
    }

    public void GoToButton(int bIndex)
    {
        targetNearestButton = false;
        minButtonNum = bIndex;
    }
}
