using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMoveCtrl : MonoBehaviour
{
    Vector3 initPos;
    float dy = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        if (p.y > initPos.y + 0.2)
        {
            dy = -0.02f;
        }
        else if(p.y < initPos.y - 0.2)
        {
            dy = 0.02f;
        }

        p.y += dy;
        transform.position = p;
    }
}
