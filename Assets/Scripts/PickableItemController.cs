using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItemController : MonoBehaviour
{
    public Sprite sprite;
    public EnergyBar eb;
    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (tag)
        {
            case "chest":
                if (sprite != null)
                    GetComponent<SpriteRenderer>().sprite = sprite;
                gm.FinishLevel();
                break;
            case "lightning":
                if (eb)
                {
                    eb.SetBarVisible(true);
                    gm.SetLightningPosY();
                }
                break;
            case "boost":
                gm.SetBoostPosY();
                break;
            case "spike":
                gm.GameOver();
                break;
        }
    }
}
