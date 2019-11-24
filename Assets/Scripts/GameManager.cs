using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerCtrl pc;
    private GUIManager guiMgr;
    private EnergyBar eb;
    public float chargeForce;
    public float maxForce;
    public bool paused;
    private float lightningPosY;
    private bool gotLightning;
    private float boostPosY;
    private bool gotBoost;
    public int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerCtrl>();
        eb = GetComponent<EnergyBar>();
        maxForce = pc.chargeTime;
        paused = false;
        gotLightning = false; ;
        guiMgr = GetComponent<GUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        chargeForce = pc.jumpCharge;

        if (gotLightning)
        {
            if(player.transform.position.y >= lightningPosY - 1)
            {
                eb.SetBarVisible(true);
            }
            else
            {
                eb.SetBarVisible(false);
            }
        }
        if (gotBoost)
        {
            if (player.transform.position.y >= boostPosY - 1)
            {
                pc.SetSpeed(0.075f);
            }
            else
            {
                pc.SetSpeed(0.05f);
            }
        }
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
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save_data.bin", FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();


            if (this.level != save.level)
            {
                StartCoroutine(LoadLevelInXSecs(save.level, 0, true));
                return;
            }
            player.transform.position = new Vector3(save.x, save.y, save.z);
            this.lightningPosY = save.lightningPosY;
            this.gotLightning = save.gotLightning;
            this.gotBoost = save.gotBoost;
            this.boostPosY = save.boostPosY;

            Debug.Log("LOAD SUCCESS level " + save.level);
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
        save.lightningPosY = lightningPosY;
        save.gotLightning = gotLightning;
        save.boostPosY = boostPosY;
        save.gotBoost = gotBoost;
        return save;
    }

    public void SetLightningPosY()
    {
        lightningPosY = player.transform.position.y;
        gotLightning = true;
    }

    public void SetBoostPosY()
    {
        boostPosY = player.transform.position.y;
        gotBoost = true;
    }

    public void LevelUp()
    {
        boostPosY = lightningPosY = 0;
        gotBoost = gotLightning = false;
    }

    public void FinishLevel()
    {
        Debug.Log("Finished!");
        if (level < 2)
        {
            NextLevel();
        }
        else
        {
            Debug.Log("No more level!");
            guiMgr.ShowNotify("CONGRATULATION", "You have finished the game!", "OKAY");
        }
    }

    void NextLevel()
    {
        StartCoroutine(LoadLevelInXSecs(this.level + 1, 5));
    }

    IEnumerator LoadLevelInXSecs(int level, int time, bool isLoad = false)
    {
        yield return new WaitForSeconds(time);
        AsyncOperation a = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        while (!a.isDone)
        {
            yield return null;
        }
        if (a.isDone)
        {
            if (isLoad)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(level);
                GameObject[] objects = scene.GetRootGameObjects();
                foreach (GameObject o in objects)
                {
                    if (o.name == "GameManager")
                    {
                        o.GetComponent<GameManager>().LoadGame();
                    }
                }
            }                

            // Unload this scene
            SceneManager.UnloadSceneAsync(this.level);
        } 
    }

    public void BackToMainMenu()
    {
        SceneManager.UnloadSceneAsync(this.level);

        Scene scene = SceneManager.GetSceneByBuildIndex(0);
        GameObject[] objects = scene.GetRootGameObjects();
        foreach (GameObject o in objects)
        {
            if (o.name == "MainMenuCtrl")
            {
                o.GetComponent<MainMenuCtrl>().BackToMainMenu();
            }
        }
    }
}
