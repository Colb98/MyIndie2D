using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float x, y, z;
    // Items here
    public int level = 1;
    public float lightningPosY;
    public bool gotLightning = false;
    public float boostPosY;
    public bool gotBoost = false;

    public void SetPosition(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }

    public override string ToString()
    {
        return "{pos: (" + x + "," + y + "," + z + "); GotLightning: " + gotLightning + "; GotBoost: " + gotBoost + "; LightningPosY: " + lightningPosY + "; BoostPosY: " + boostPosY;
    }
}
