using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private PlayerCtrl pc;
    private GUIManager guiMgr;
    private EnergyBar eb;
    private AudioSource music;
    private AudioSource sfx;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    public float chargeForce;
    public float maxForce;
    public bool paused;
    private float lightningPosY;
    private bool gotLightning;
    private float boostPosY;
    private bool gotBoost;
    private Vector3 initPos;
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
        initPos = new Vector2(-1.46f, -3.93f);
    }

    // Update is called once per frame
    void Update()
    {
        chargeForce = pc.jumpCharge;

        if (gotLightning)
        {
            if(player.transform.position.y >= lightningPosY - 3)
            {
                eb.SetBarVisible(true);
            }
            else
            {
                eb.SetBarVisible(false);
            }
        }
        else
        {
            eb.SetBarVisible(false);
        }

        if (gotBoost)
        {
            if (player.transform.position.y >= boostPosY - 3)
            {
                pc.SetSpeed(0.075f);
            }
            else
            {
                pc.SetSpeed(0.05f);
            }
        }
        else
        {
            pc.SetSpeed(0.05f);
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

        Debug.Log("Game saved: " + save.ToString());

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
            Debug.Log("Game loaded: " + save.ToString());
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

    public void GameOver()
    {
        guiMgr.ShowNotify("GAME OVER", "You died :( \n Try again?", "OKAY");
        ResetLevel();
    }

    public void ResetLevel()
    {
        pc.TeleportToPoint(initPos);
        gotBoost = false;
        gotLightning = false;
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
                        GameManager g = o.GetComponent<GameManager>();
                        g.LoadGame();
                        g.SetAudioSource(this.music, this.sfx);
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

    public void SetAudioSource(AudioSource music, AudioSource sfx)
    {
        this.music = music;
        this.sfx = sfx;
    }

    public void PlaySFX()
    {
        if (sfx)
            this.sfx.Play();
    }

    public void UpdateMusicSFX()
    {
        if (music)
        {
            music.mute = !musicToggle.isOn;
            sfx.mute = !sfxToggle.isOn;
        }
    }

    public void UpdateMusicSFXToggle()
    {
        if (music)
        {
            musicToggle.isOn = !music.mute;
            sfxToggle.isOn = !sfx.mute;
        }
    }
}
