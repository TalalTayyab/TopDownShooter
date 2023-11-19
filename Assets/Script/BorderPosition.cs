using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BorderPosition : MonoBehaviour
{
    [SerializeField] private int location = 0;
    [SerializeField] private bool firsthalf = false;

    public Vector3 TopLeft;
    public Vector3 BottomRight;
    public float MiddleY;
    public float MiddleX;

    // Start is called before the first frame update
    void Start()
    {
        var camera = Camera.main;
        var pos = transform.position;

        TopLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
        BottomRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, transform.position.z));
        MiddleY =  (BottomRight.y - TopLeft.y) / 2f;
        MiddleX = (BottomRight.x - TopLeft.x) / 2f;

        if (location == 0) //top
        {
            if (firsthalf)
            {
                transform.position = new Vector3(MiddleX, TopLeft.y, pos.z);
            }
            else
            {
                transform.position = new Vector3(-MiddleX, TopLeft.y, pos.z);
            }

            transform.localScale = new Vector3(MiddleX * 2f, transform.localScale.y, transform.localScale.z);
        }

        if (location == 1) //bottom
        {
            if (firsthalf)
            {
                transform.position = new Vector3(MiddleX, BottomRight.y, pos.z);
            }
            else
            {
                transform.position = new Vector3(-MiddleX, BottomRight.y, pos.z);
            }

            transform.localScale = new Vector3(MiddleX * 2f, transform.localScale.y, transform.localScale.z);
        }

        if (location == 2) //left
        {
            if (firsthalf)
            {
                transform.position = new Vector3(TopLeft.x, MiddleY, pos.z);
            }
            else
            {
                transform.position = new Vector3(TopLeft.x, -MiddleY, pos.z);
            }

            transform.localScale = new Vector3(transform.localScale.x, MiddleY * 2f, transform.localScale.z);
        }

        if (location == 3) //right
        {
            if (firsthalf)
            {
                transform.position = new Vector3(BottomRight.x, MiddleY, pos.z);
            }
            else
            {
                transform.position = new Vector3(BottomRight.x, -MiddleY, pos.z);
            }

            transform.localScale = new Vector3(transform.localScale.x, MiddleY * 2f, transform.localScale.z);
        }
    }


}
