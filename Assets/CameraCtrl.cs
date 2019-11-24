using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Camera camera;
    Vector2 offset;
    float initZ;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2();
        offset.x = camera.transform.position.x - transform.position.x;
        offset.y = camera.transform.position.y - transform.position.y;
        initZ = camera.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, initZ);
        pos.x += offset.x;
        pos.y += offset.y;

        if(pos.x > 2)
        {
            pos.x = 2;
            offset.x = pos.x - transform.position.x;
        }
        if(pos.x < 0)
        {
            pos.x = 0;
            offset.x = pos.x - transform.position.x;
        }

        if(transform.position.y > 53)
        {
            pos.y = 53 + offset.y;
        }

        camera.transform.position = pos;
    }
}
