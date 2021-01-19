using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggler : MonoBehaviour
{
    private float yPos;
    private float yOrig;

    private float top;
    private float bot;

    private float speed = 0.8f;

    private Vector3 newPosition;

    private void Start()
    {
        yOrig = transform.position.y;
        top = yOrig + 0.3f;
        bot = yOrig - 0.3f;

        newPosition = new Vector3(0,1,0);
    }

    private void Update()
    {
        yPos = transform.position.y;

        if (yPos > top)
        {
            newPosition = new Vector3(0, -1, 0);
        }
        else if (yPos < bot)
        {
            yPos++;
            newPosition = new Vector3(0, 1, 0);
        }


        transform.Translate(newPosition * Time.deltaTime * speed);
    }
}
