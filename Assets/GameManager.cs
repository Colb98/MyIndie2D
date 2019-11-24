using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerCtrl pc;
    public float chargeForce;
    public float maxForce;
    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerCtrl>();
        maxForce = pc.chargeTime;
    }

    // Update is called once per frame
    void Update()
    {
        chargeForce = pc.jumpCharge;
    }
}
