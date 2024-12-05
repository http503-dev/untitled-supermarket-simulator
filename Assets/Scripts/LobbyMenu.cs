using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenu : MonoBehaviour
{
    public Button readyButton;
    public Image vrImage;
    public Image webImage;
    public TextMeshProUGUI vrReadyText;
    public TextMeshProUGUI webReadyText;

    private void Start()
    {
        DatabaseManager.instance.GameSessionListen();
    }

    public void OnVRReady()
    {
        vrImage.GetComponent<Image>().color = new Color32(50, 236, 196, 255);
        vrReadyText.text = "Player 1: VR\r\nReady";
    }

    public void ChangeWebReadyStatus(bool webReadyStatus)
    {
        Debug.Log(webReadyStatus);
        if (webReadyStatus)
        {
            webImage.GetComponent<Image>().color = new Color32(50, 236, 196, 255);
            webReadyText.text = "Player 2: Web\r\nReady";
        }
        else
        {
            webImage.GetComponent<Image>().color = new Color32(238, 50, 83, 255);
            webReadyText.text = "Player 2: Web\r\nNot Ready";
        }
    }


}
