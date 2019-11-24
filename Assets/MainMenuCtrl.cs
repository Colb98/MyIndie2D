using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCtrl : MonoBehaviour
{
    public Scene reference;
    public GameObject mainMenu;
    public GameObject optionMenu;
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
                    o.GetComponent<GameManager>().LoadGame();
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
    }

    private void HideAll()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(false);
    }
}
