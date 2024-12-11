/*
 * Author: Muhammad Farhan
 * Date: 4/12/2024
 * Description: Function to spawn customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShiftTimer : MonoBehaviour
{
    /// <summary>
    /// To determine how long a shift is and references clock text
    /// </summary>
    public float shiftDuration = 480f; // 8 minutes in seconds
    public TextMeshProUGUI clockText; // Link a UI Text element for displaying the virtual clock
    public GameObject mainMenuButton;
    public GameObject newShiftButton;

    /// <summary>
    /// To track elapsed time and whther there is a shift
    /// </summary>
    private float elapsedTime;
    private bool isShiftActive;

    /// <summary>
    /// Reference to ShiftDataTracker
    /// </summary>
    public ShiftDataTracker shiftDataTracker;
    private string userId;

    /// <summary>
    /// Set elapsed time to 0 and that a shift has started 
    /// </summary>
    void Start()
    {
        userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User ID not found. Ensure the user is authenticated.");
            return;
        }

        mainMenuButton.SetActive(false);
        newShiftButton.SetActive(false);

        elapsedTime = 0f;
        isShiftActive = true;
        
        shiftDataTracker.StartShift(userId);
    }

    /// <summary>
    /// To run update function every frame
    /// </summary>
    void Update()
    {
        if (isShiftActive)
        {
            UpdateClock();
        }
    }

    /// <summary>
    /// Function to update the clock when needed
    /// </summary>
    void UpdateClock()
    {
        if (elapsedTime < shiftDuration)
        {
            elapsedTime += Time.deltaTime;
            float virtualHours = Mathf.Lerp(9f, 17f, elapsedTime / shiftDuration); // Map elapsed time to 9 AM to 5 PM
            int hours = Mathf.FloorToInt(virtualHours);
            int minutes = Mathf.FloorToInt((virtualHours - hours) * 60);
            clockText.text = $"{hours}:{minutes:D2}";
        }
        else
        {
            EndShift();
        }
    }

    /// <summary>
    /// Function to end shift
    /// </summary>
    void EndShift()
    {
        isShiftActive = false;
        clockText.text = "Shift Over!";
        elapsedTime = 0f;

        // Finalize stats in ShiftDataTracker
        shiftDataTracker.EndShift(userId);

        mainMenuButton.SetActive(true);
        newShiftButton.SetActive(true);

        Debug.Log("Shift ended successfully.");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartNewShift()
    {
        SceneManager.LoadScene("Supermarket");
    }
}
