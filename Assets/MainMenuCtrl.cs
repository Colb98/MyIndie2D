using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour
{
    public Scene reference;
    public GameObject mainMenu;
    public GameObject optionMenu;
    public AudioSource music;
    public AudioSource sfx;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene(1));
    }

    public void LoadGame()
    {
        StartCoroutine(LoadSave());
    }

    IEnumerator LoadScene(int index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
            this.reference = SceneManager.GetSceneByBuildIndex(index);
            HideAll();

            Scene scene = SceneManager.GetSceneByBuildIndex(1);
            GameObject[] objects = scene.GetRootGameObjects();
            foreach (GameObject o in objects)
            {
                if (o.name == "GameManager")
                {
                    GameManager g = o.GetComponent<GameManager>();
                    g.SetAudioSource(this.music, this.sfx);
                }
            }
        }
    }

    IEnumerator LoadSave()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
            Scene scene = SceneManager.GetSceneByBuildIndex(1);
            GameObject[] objects = scene.GetRootGameObjects();
            foreach(GameObject o in objects)
            {
                if(o.name == "GameManager")
                {
                    GameManager g = o.GetComponent<GameManager>();
                    g.LoadGame();
                    g.SetAudioSource(this.music, this.sfx);
                }
            }

            // Unload this scene
            //SceneManager.UnloadSceneAsync(0);
            HideAll();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowOption()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive(false);

        musicToggle.isOn = !music.mute;
        sfxToggle.isOn = !sfx.mute;
    }

    private void HideAll()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    public void SetMusicEnabled()
    {
        if (music)
        {
            music.mute = !musicToggle.isOn;
        }
    }

    public void SetSFXEnabled()
    {
        if (music)
        {
            sfx.mute = !sfxToggle.isOn;
        }
    }
}
