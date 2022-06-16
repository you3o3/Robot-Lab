using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private RectTransform rect1, rect2;

    // Update is called once per frame
    void Update()
    {
        float x1 = rect1.anchoredPosition.x - speed;
        if (rect1.anchoredPosition.x <= 0 - 1000 - 1600) // window width = 1600 x 900
        {
            x1 = 1000;
        }
        Vector2 newPos1 = new Vector2(x1, rect1.anchoredPosition.y);
        rect1.anchoredPosition = newPos1;

        // technique: deduce the distance from rect1: see initial pos by Debug.Log(rect1.anchoredPosition.x);
        // and calculate the difference of init x pos and that in line 15
        float x2 = rect2.anchoredPosition.x - speed;
        if (rect1.anchoredPosition.x >= 3600 - 1000 - 1600)
        {
            x2 = 3600 + 1000;
        }
        Vector2 newPos2 = new Vector2(x2, rect2.anchoredPosition.y);
        rect2.anchoredPosition = newPos2;
    }

}
