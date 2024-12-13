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
    public NPCSpawner spawner;

    /// <summary>
    /// Reference to set queueing npcs to an array
    /// </summary>
    public Queue<NPCController> queue = new Queue<NPCController>();
    private bool isCheckoutBusy = false;

    private void OnTriggerEnter(Collider other)
    {
        NPCController npcController = other.GetComponent<NPCController>();
        if (npcController != null && npcController.currentState == NPCController.NPCState.Checkout)
        {
            if (isCheckoutBusy)
            {
                queue.Enqueue(npcController);
                Debug.Log($"{npcController.CustomerData.fullName} is waiting in the queue.");
            }
            else
            {
                ServeCustomer(npcController);
            }
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

        isCheckoutBusy = false;

        if (queue.Count > 0)
        {
            NPCController nextNpc = queue.Dequeue();
            ServeCustomer(nextNpc);
        }
    }

    /// <summary>
    /// Function to start serving the customer 
    /// </summary>
    /// <param name="npc"></param>
    public void ServeCustomer(NPCController npc)
    {
        isCheckoutBusy = true;

        // Assign the customer to the CheckoutManager
        CheckoutManager.Instance.AssignCustomer(npc.CustomerData);

        // Spawn the customer's items
        CheckoutManager.Instance.SpawnItemsForCustomer(npc.CustomerData);

        // Send customer data to database
        spawner.SaveCustomerToDatabase(npc.CustomerData);

        // Start the transaction timer
        transactionStartTime = Time.time;
        Debug.Log($"Transaction started for customer: {npc.CustomerData.fullName}");
    }
}
