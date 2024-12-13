/*
 * Author: Muhammad Farhan
 * Date: 13/12/2024
 * Description: Script to handle queueing customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private Queue<NPCController> customerQueue = new Queue<NPCController>(); // Reference to array of queueing customers
    public CheckoutManager checkoutManager; // Reference to checkout manager script

    /// <summary>
    /// Function to dynamically add customers to queue
    /// </summary>
    /// <param name="npc"></param>
    public void AddToQueue(NPCController npc)
    {
        // Add the NPC to the queue
        customerQueue.Enqueue(npc);
        Debug.Log($"{npc.CustomerData.fullName} added to the queue.");

        // Serve the customer immediately if checkout is not busy
        if (!checkoutManager.IsCheckoutBusy)
        {
            ServeNextCustomer();
        }
    }

    /// <summary>
    /// Function to let the next customer in queue enter checkoout
    /// </summary>
    public void ServeNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            NPCController nextCustomer = customerQueue.Dequeue();
            checkoutManager.AssignCustomer(nextCustomer.CustomerData);

            // Let the NPC know they are being served (e.g., set state to Checkout)
            nextCustomer.StartCheckout();
        }
        else
        {
            Debug.Log("No customers in queue.");
        }
    }
}
