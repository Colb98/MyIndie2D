using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public RectTransform mask;
    public GameObject Bar;
    private Rect rect;
    
    public float percent;
    // Start is called before the first frame update
    void Start()
    {
        rect = mask.rect;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gm = GetComponent<GameManager>();
        percent = gm.chargeForce / gm.maxForce;

        mask.sizeDelta = new Vector2(rect.width, percent * rect.height);
    }

    public void SetBarVisible(bool visible)
    {
        //Debug.Log("set visible " + visible);
        Bar.SetActive(visible);
    }
}
