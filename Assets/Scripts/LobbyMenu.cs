using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    public Button readyButton;
    public Image vrImage;
    public Image webImage;
    public TextMeshProUGUI vrReadyText;
    public TextMeshProUGUI webReadyText;
    public bool vrReadyStatus;
    public bool webReadyStatus;

    private void Start()
    {
        DatabaseManager.instance.GameSessionListen();
    }
    private void Update()
    {
        if (vrReadyStatus && webReadyStatus) 
        {
            Debug.Log("nice");
            SceneManager.LoadScene("Supermarket");
        }
    }
    public void OnVRReady()
    {
        vrImage.GetComponent<Image>().color = new Color32(50, 236, 196, 255);
        vrReadyText.text = "Player 1: VR\r\nReady";
        DatabaseManager.instance.UpdateVRReadyStatus(true);
        vrReadyStatus = true;
    }



    public void ChangeWebReadyStatus(bool webStatus)
    {
        Debug.Log(webStatus);
        if (webStatus)
        {
            webImage.GetComponent<Image>().color = new Color32(50, 236, 196, 255);
            webReadyText.text = "Player 2: Web\r\nReady";
            webReadyStatus = true;
        }
        else
        {
            webImage.GetComponent<Image>().color = new Color32(238, 50, 83, 255);
            webReadyText.text = "Player 2: Web\r\nNot Ready";
            webReadyStatus = false;
        }
    }

}
