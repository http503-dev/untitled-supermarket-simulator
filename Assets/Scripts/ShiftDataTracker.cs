/*
 * Author: Muhammad Farhan
 * Date: 5/12/2024
 * Description: Script to retrieve/update player stats to database
 */
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Firebase;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ShiftDataTracker : MonoBehaviour
{
    /// <summary>
    /// List of variables to track/store
    /// </summary>
    // Tracking Variables
    private int customersServed = 0;
    private int itemsScanned = 0;
    private int mistakesMade = 0;
    private int restrictedSalesToMinors = 0;
    private int excessChangeGiven = 0;
    private int insufficientChangeGiven = 0;
    private int customerOvercharged = 0;
    private int customerUndercharged = 0;
    private int insufficientCustomerPayment = 0;
    private float profitsEarned = 0f;
    private int shiftsCompleted = 0;
    private float totalTimeSpent = 0f;
    private float averageTimePerTransaction = 480f; // Default 480 seconds
    private int highScore = 0;
    private int proficiencyScore = 0;

    /// <summary>
    /// Dictionary to store data from database
    /// </summary>
    private Dictionary<string, object> snapshotData = new Dictionary<string, object>();

    /// <summary>
    /// Float variable to track transaction time
    /// </summary>
    private float transactionStartTime;

    /// <summary>
    /// Firebase reference
    /// </summary>
    private DatabaseReference reference;

    /// <summary>
    /// To set database reference and reset tracked data for new playthrough
    /// </summary>
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        // Initialize tracking variables at the start of a shift
        ResetTrackingData();
    }

    /// <summary>
    /// Function to take a snapshot of existing player data at the start of the shift
    /// </summary>
    /// <param name="uid"></param>
    public void StartShift(string uid)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("players").Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Store top-level data points
                snapshotData["shiftsCompleted"] = snapshot.Child("shiftsCompleted").Value != null
                    ? int.Parse(snapshot.Child("shiftsCompleted").Value.ToString()) : 0;

                snapshotData["customersServed"] = snapshot.Child("customersServed").Value != null
                    ? int.Parse(snapshot.Child("customersServed").Value.ToString()) : 0;

                snapshotData["itemsScanned"] = snapshot.Child("itemsScanned").Value != null
                    ? int.Parse(snapshot.Child("itemsScanned").Value.ToString()) : 0;

                snapshotData["profitsEarned"] = snapshot.Child("profitsEarned").Value != null
                    ? float.Parse(snapshot.Child("profitsEarned").Value.ToString()) : 0f;

                snapshotData["highScore"] = snapshot.Child("highScore").Value != null
                    ? int.Parse(snapshot.Child("highScore").Value.ToString()) : 0;

                snapshotData["proficiencyScore"] = snapshot.Child("proficiencyScore").Value != null
                    ? int.Parse(snapshot.Child("proficiencyScore").Value.ToString()) : 0;

                snapshotData["averageTimePerTransaction"] = snapshot.Child("averageTimePerTransaction").Value != null
                    ? float.Parse(snapshot.Child("averageTimePerTransaction").Value.ToString()) : 480f;

                // Store nested structure: mistakesMade
                if (snapshot.Child("mistakesMade").Exists)
                {
                    snapshotData["mistakesMade"] = new Dictionary<string, int>
                    {
                        ["restrictedSalesToMinors"] = snapshot.Child("mistakesMade/restrictedSalesToMinors").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/restrictedSalesToMinors").Value.ToString()) : 0,
                        ["excessChangeGiven"] = snapshot.Child("mistakesMade/excessChangeGiven").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/excessChangeGiven").Value.ToString()) : 0,
                        ["insufficientChangeGiven"] = snapshot.Child("mistakesMade/insufficientChangeGiven").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/insufficientChangeGiven").Value.ToString()) : 0,
                        ["customersOvercharged"] = snapshot.Child("mistakesMade/customersOvercharged").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/customersOvercharged").Value.ToString()) : 0,
                        ["customersUndercharged"] = snapshot.Child("mistakesMade/customersUndercharged").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/customersUndercharged").Value.ToString()) : 0,
                        ["insufficientCustomerPayment"] = snapshot.Child("mistakesMade/insufficientCustomerPayment").Value != null
                            ? int.Parse(snapshot.Child("mistakesMade/insufficientCustomerPayment").Value.ToString()) : 0,
                    };
                }
                else
                {
                    snapshotData["mistakesMade"] = new Dictionary<string, int>
                    {
                        ["restrictedSalesToMinors"] = 0, //
                        ["excessChangeGiven"] = 0, //
                        ["insufficientChangeGiven"] = 0, //
                        ["customersOvercharged"] = 0,
                        ["customersUndercharged"] = 0,
                        ["insufficientCustomerPayment"] = 0, //
                    };
                }

                Debug.Log("Snapshot taken successfully.");
            }
            else
            {
                Debug.LogError("Failed to take snapshot: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// Function to start transaction timer
    /// </summary>
    public void StartTransaction()
    {
        transactionStartTime = Time.time;  // Start the timer when the customer enters
        Debug.Log("Transaction started for a new customer.");
    }

    /// <summary>
    /// Function to track scanned items
    /// </summary>
    /// <param name="itemPrice"></param>
    /// <param name="isOvercharged"></param>
    /// <param name="isUndercharged"></param>
    public void TrackItemScan(float itemPrice, bool isOvercharged, bool isUndercharged)
    {
        itemsScanned++; // Increment items scanned
        profitsEarned += itemPrice; // Add to profits earned

        if (isOvercharged)
        {
            customerOvercharged++;
            profitsEarned -= itemPrice; // Deduct overcharged profit
            Debug.Log("Overcharge detected.");
        }

        if (isUndercharged)
        {
            customerUndercharged++;
            profitsEarned -= itemPrice; // Deduct undercharged profit
            Debug.Log("Undercharge detected.");
        }

        Debug.Log($"Item scanned: ${itemPrice}. Total items scanned: {itemsScanned}. Current profits: ${profitsEarned}.");
    }

    /// <summary>
    /// Function to track various mistakes
    /// </summary>
    /// <param name="mistakeType"></param>
    public void TrackMistake(string mistakeType)
    {
        mistakesMade++; // Increment total mistakes

        switch (mistakeType)
        {
            case "RestrictedSale":
                restrictedSalesToMinors++;
                Debug.Log("Restricted sale to minors detected.");
                break;
            case "ExcessChange":
                excessChangeGiven++;
                Debug.Log("Excess change given detected.");
                break;
            case "InsufficientChange":
                insufficientChangeGiven++;
                Debug.Log("Insufficient change given detected.");
                break;
            case "InsufficientPayment":
                insufficientCustomerPayment++;
                Debug.Log("Insufficient customer payment detected.");
                break;
            case "CustomerOvercharged":
                customerOvercharged++;
                Debug.Log("Customer was overcharged.");
                break;
            case "CustomerUndercharged":
                customerUndercharged++;
                Debug.Log("Customer was undercharged.");
                break;
        }
    }

    /// <summary>
    /// Function to complete transaction
    /// </summary>
    /// <param name="transactionTime"></param>
    public void CompleteTransaction(float transactionTime)
    {
        customersServed++; // Increment customers served
        totalTimeSpent += transactionTime; // Add transaction time to total time

        Debug.Log($"Customer served. Transaction time: {transactionTime:F2} seconds. Total customers: {customersServed}.");
    }

    /// <summary>
    /// Function to end shift and update database
    /// </summary>
    /// <param name="uid"></param>
    public void EndShift(string uid)
    {
        // Calculate the metrics for the shift
        shiftsCompleted++;
        averageTimePerTransaction = customersServed > 0
            ? totalTimeSpent / customersServed
            : 480f; // Default if no valid transactions

        proficiencyScore = customersServed > 0 && mistakesMade > 0
            ? Mathf.RoundToInt((customersServed / ((mistakesMade + 1) * averageTimePerTransaction)) * 100)
            : 0;

        if (profitsEarned > highScore)
        {
            highScore = (int)profitsEarned;
        }

        // Retrieve and increment values using the snapshot
        int updatedShiftsCompleted = (int)snapshotData["shiftsCompleted"] + shiftsCompleted;
        int updatedCustomersServed = (int)snapshotData["customersServed"] + customersServed;
        int updatedItemsScanned = (int)snapshotData["itemsScanned"] + itemsScanned;
        float updatedProfitsEarned = (float)snapshotData["profitsEarned"] + profitsEarned;

        // Update mistakes individually
        var snapshotMistakes = (Dictionary<string, int>)snapshotData["mistakesMade"];
        Dictionary<string, int> updatedMistakes = new Dictionary<string, int>
        {
            ["restrictedSalesToMinors"] = snapshotMistakes["restrictedSalesToMinors"] + restrictedSalesToMinors,
            ["excessChangeGiven"] = snapshotMistakes["excessChangeGiven"] + excessChangeGiven,
            ["insufficientChangeGiven"] = snapshotMistakes["insufficientChangeGiven"] + insufficientChangeGiven,
            ["customersOvercharged"] = snapshotMistakes["customersOvercharged"] + customerOvercharged,
            ["customersUndercharged"] = snapshotMistakes["customersUndercharged"] + customerUndercharged,
            ["insufficientCustomerPayment"] = snapshotMistakes["insufficientCustomerPayment"] + insufficientCustomerPayment
        };

        // Prepare the data for updating
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            ["shiftsCompleted"] = updatedShiftsCompleted,
            ["customersServed"] = updatedCustomersServed,
            ["itemsScanned"] = updatedItemsScanned,
            ["profitsEarned"] = updatedProfitsEarned,
            ["highScore"] = highScore,
            ["proficiencyScore"] = proficiencyScore,
            ["averageTimePerTransaction"] = averageTimePerTransaction
        };

        // Add nested mistakes data
        foreach (var mistake in updatedMistakes)
        {
            updates[$"mistakesMade/{mistake.Key}"] = mistake.Value;
        }

        // Push the updates to Firebase
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("players").Child(uid).UpdateChildrenAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Shift data updated successfully.");
            }
            else
            {
                Debug.LogError("Error updating shift data: " + task.Exception);
            }
        });

        // Reset local shift tracking variables
        ResetTrackingData();
    }

    /// <summary>
    /// Function to reset tracking data
    /// </summary>
    public void ResetTrackingData()
    {
        customersServed = 0;
        itemsScanned = 0;
        mistakesMade = 0;
        restrictedSalesToMinors = 0;
        excessChangeGiven = 0;
        insufficientChangeGiven = 0;
        customerOvercharged = 0;
        customerUndercharged = 0;
        insufficientCustomerPayment = 0;
        profitsEarned = 0f;
        totalTimeSpent = 0f;
        averageTimePerTransaction = 480f;
        proficiencyScore = 0;
    }
}
