using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float x, y, z;
    // Items here
    public int level = 1;

    public void SetPosition(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
}
