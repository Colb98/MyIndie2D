using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMoveCtrl : MonoBehaviour
{
    Vector3 initPos;
    public bool moveVertical = true;
    public float range = 0.2f;
    public float speed = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;

        if (moveVertical)
        {
            if (p.y > initPos.y + range)
            {
                speed = -0.02f;
            }
            else if (p.y < initPos.y - range)
            {
                speed = 0.02f;
            }

            p.y += speed;
        }
        else
        {
            if (p.x > initPos.x + range)
            {
                speed = -0.02f;
            }
            else if (p.x < initPos.x - range)
            {
                speed = 0.02f;
            }
            p.x += speed;
        }

        transform.position = p;
    }
}
