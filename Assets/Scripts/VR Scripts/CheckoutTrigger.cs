/*
 * Author: Muhammad Farhan
 * Date: 30/11/2024
 * Description: Script to check if customer enters checkout trigger leading to spawning of customer's groceries
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutTrigger : MonoBehaviour
{
    private float transactionStartTime; // Track the start time of each transaction
    public ShiftDataTracker shiftDataTracker; // Reference to ShiftDataTracker

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is a customer
        NPCController npcController = other.GetComponent<NPCController>();
        if (npcController != null)
        {
            // Assign the customer to the CheckoutManager
            CheckoutManager.Instance.AssignCustomer(npcController.CustomerData);

            // Spawn the customer's items
            CheckoutManager.Instance.SpawnItemsForCustomer(npcController.CustomerData);

            // Start the transaction timer
            transactionStartTime = Time.time;
            Debug.Log($"Transaction started for customer: {npcController.CustomerData.FullName}");
        }
    }

    /// <summary>
    /// Function to complete transaction
    /// </summary>
    public void CompleteTransaction()
    {
        // Calculate transaction duration
        float transactionDuration = Time.time - transactionStartTime;

        // Log to ShiftDataTracker
        shiftDataTracker.CompleteTransaction(transactionDuration);

        Debug.Log($"Transaction completed. Duration: {transactionDuration:F2} seconds.");
    }
}
