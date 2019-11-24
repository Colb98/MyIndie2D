using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerCtrl pc;
    private GUIManager guiMgr;
    public float chargeForce;
    public float maxForce;
    public bool paused;
    public int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerCtrl>();
        maxForce = pc.chargeTime;
        paused = false;
        guiMgr = GetComponent<GUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        chargeForce = pc.jumpCharge;
    }

    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SaveGame()
    {
        Debug.Log("Player position: " + Utils.VectorToString(pc.lastGroundPos));
        SaveData save = CreateSaveData();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save_data.bin");
        bf.Serialize(file, save);
        file.Close();
    }

    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/save_data.bin"))
        {

        }
        else
        {
            Debug.Log("No save data");
            guiMgr.ShowNotify("Notify", "There's no save data!", "OK");
        }
    }

    private SaveData CreateSaveData()
    {
        SaveData save = new SaveData();

        save.SetPosition(pc.lastGroundPos);
        save.level = level;
        return save;
    }
}
