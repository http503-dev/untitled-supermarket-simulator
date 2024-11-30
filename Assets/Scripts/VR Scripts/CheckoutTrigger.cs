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
        }
    }
}
