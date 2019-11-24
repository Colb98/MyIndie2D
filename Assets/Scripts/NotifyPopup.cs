using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotifyPopup : MonoBehaviour
{
    public Text title;
    public Text msg;
    public Text button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTitle(string s)
    {
        title.text = s;
    }

    public void SetMessage(string s)
    {
        msg.text = s;
    }

    public void SetButton(string s)
    {
        button.text = s;
    }

    public void SetAll(string s1, string s2, string s3)
    {
        SetTitle(s1);
        SetMessage(s2);
        SetButton(s3);
    }

    public void Show()
    {
        enabled = true;
    }

    public void Hide()
    {
        enabled = false;
    }
}
