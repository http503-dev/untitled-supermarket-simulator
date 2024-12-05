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
    public void OnVRReady()
    {
        vrImage.GetComponent<Image>().color = new Color32(50, 236, 196, 255);
        vrReadyText.text = "Player 1: VR\r\nReady";
    }


}
