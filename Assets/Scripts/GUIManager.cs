using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GUIManager : MonoBehaviour
{
    public GameObject quitConfirmPopup;
    public GameObject pausePopup;
    public NotifyPopup notifyPopup;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        SetExitConfirmVisible(false);
        SetPauseVisible(false);
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        SetPauseVisible(true);
        gm.PauseGame();
    }

    public void SetExitConfirmVisible(bool visible)
    {
        Debug.Log("Show or hide " + visible);
        if (quitConfirmPopup)
        {
            quitConfirmPopup.SetActive(visible);
        }
        if (pausePopup)
        {
            pausePopup.SetActive(!visible);
        }
    }

    public void SetPauseVisible(bool visible)
    {
        if (pausePopup)
        {
            pausePopup.SetActive(visible);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        SetPauseVisible(false);
        if (gm.paused)
        {
            gm.PauseGame();
        }
    }

    public void ShowNotify(string title, string msg, string button)
    {
        notifyPopup.SetAll(title, msg, button);
        notifyPopup.Show();
    }

    public void SaveAndQuit()
    {
        gm.SaveGame();

        ExitGame();
    }
}
