/*
 * Author: Muhammad Farhan
 * Date: 4/12/2024
 * Description: Function to spawn customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShiftTimer : MonoBehaviour
{
    /// <summary>
    /// To determine how long a shift is and references clock text
    /// </summary>
    public float shiftDuration = 480f; // 8 minutes in seconds
    public TextMeshProUGUI clockText; // Link a UI Text element for displaying the virtual clock

    /// <summary>
    /// To track elapsed time and whther there is a shift
    /// </summary>
    private float elapsedTime;
    private bool isShiftActive;

    /// <summary>
    /// Set elapsed time to 0 and that a shift has started 
    /// </summary>
    void Start()
    {
        elapsedTime = 0f;
        isShiftActive = true;
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
            clockText.text = $"{hours}:00"; // Display only hours
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
        // Add logic to handle the end of the shift
    }
}
