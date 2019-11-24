using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    public GameObject highBg;
    public GameObject lowBg;
    public Camera cam;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = lowBg.transform.position.y - transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = lowBg.transform.position;
        if (transform.position.y < 28)
        {
            pos.y = transform.position.y + offset;
            lowBg.transform.position = pos;
        }
    }
}
