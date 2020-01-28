using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TMP_InputField IpInput, PortInput;
    public Button button;
    public TMP_Text buttonText;

    public GameObject pointer;
    CursorManager cursor;

    public GameObject menu;
    Animator menuAnimator;

    string IP = "192.168.0.102";
    int port = 8052;

    void Start()
    {
        cursor = pointer.GetComponent<CursorManager>();
        menuAnimator = menu.GetComponent<Animator>();
    }
    void Update()
    {
        if (cursor.isConnected() && Input.GetKeyDown(KeyCode.Space))
        {
            bool open = menuAnimator.GetBool("Open");
            menuAnimator.SetBool("Open", !open);
        }

        if (!cursor.isConnected() && !menuAnimator.GetBool("Open")) menuAnimator.SetBool("Open", true);
    }

    public void Restart()
    {
        if(cursor.isConnected()) cursor.ServerStop();
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        if(cursor.isConnected()) cursor.ServerStop();
        Application.Quit();
    }

    public void OnToggle()
    {
        if (!cursor.isConnected()) OnConnect();
        else OnDisconnect();
    }

    void OnConnect()
    {
        if (!string.IsNullOrEmpty(IpInput.text)) IP = IpInput.text;
        if (!string.IsNullOrEmpty(PortInput.text)) port = int.Parse(PortInput.text);
        cursor.ServerStart(IP, port);

        buttonText.text = "Disconnect";

        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = new Color32(255, 105, 98, 255);
        colorBlock.highlightedColor = new Color32(236, 80, 83, 255);
        button.colors = colorBlock;

        EventSystem.current.SetSelectedGameObject(null);
        menuAnimator.SetBool("Open", false);
    }
    void OnDisconnect()
    {
        if(cursor.isConnected()) cursor.ServerStop();

        buttonText.text = "Connect";

        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = new Color32(0, 0, 0, 255);
        colorBlock.highlightedColor = new Color32(0, 149, 160, 255);
        button.colors = colorBlock;

        EventSystem.current.SetSelectedGameObject(null);
    }
}
