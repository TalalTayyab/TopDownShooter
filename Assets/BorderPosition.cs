using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPosition : MonoBehaviour
{
    [SerializeField] private int location = 0;

    // Start is called before the first frame update
    void Start()
    {
        var camera = Camera.main;
        var pos = transform.position;
        var camPosTopLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
        var camPosBottomRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, transform.position.z));

        if (location == 0) //top
        {
            transform.position = new Vector3(pos.x, camPosTopLeft.y, pos.z);
        }

        if (location==1) //bottom
        {
            transform.position = new Vector3(pos.x, camPosBottomRight.y, pos.z);
        }

        if (location == 2) //left
        {
            transform.position = new Vector3(camPosTopLeft.x, pos.y, pos.z);
            Debug.Log(transform.position);
        }

        if (location==3) //right
        {
           transform.position = new Vector3(camPosBottomRight.x, pos.y, pos.z);
        }
    }


}
