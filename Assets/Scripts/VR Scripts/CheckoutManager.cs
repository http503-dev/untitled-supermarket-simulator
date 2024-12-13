/*
 * Author: Muhammad Farhan
 * Date: 27/11/2024
 * Description: Script to handle checkout
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CustomerData;

public class CheckoutManager : MonoBehaviour
{
    /// <summary>
    /// To set checkout manager instance
    /// </summary>
    public static CheckoutManager Instance; // Singleton pattern

    public List<BarcodeItem> scannedItems = new List<BarcodeItem>();
    public float totalPrice = 0f;

    /// <summary>
    /// Placing instatiated grocery items
    /// </summary>
    public Transform spawnPoint; // Position where items will spawn
    public float itemSpacing = 0.5f; // Space between items
    private float currentOffset = 0f; // Tracks where to place the next item

    public CustomerData currentCustomer;
    private float customerPayment = 0f;
    public ShiftDataTracker shiftDataTracker; // Reference to ShiftDataTracker

    public CheckoutUI worldSpaceUI; // Reference to the UI script for displaying information

    private List<GameObject> spawnedItems = new List<GameObject>();
    public bool IsCheckoutBusy => currentCustomer != null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Function to assign customer that walked up to counter
    /// </summary>
    /// <param name="customer"></param>
    public void AssignCustomer(CustomerData customer)
    {
        currentCustomer = customer;
        Debug.Log($"Assigned customer: {customer.fullName}");
        worldSpaceUI.ShowRejectCustomerButton();
    }

    /// <summary>
    /// Function to add items scanned to a cart
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToCart(BarcodeItem item)
    {
        scannedItems.Add(item);
        totalPrice += item.itemPrice;

        // Increment items scanned
        shiftDataTracker.TrackItemScan(item.itemPrice, false, false);

        // Add the item to the UI with a linked void button
        worldSpaceUI.AddScannedItem(item.itemName, item.itemPrice, item);
        // Update the displayed total price
        worldSpaceUI.UpdateTotalPrice(totalPrice);
    }

    /// <summary>
    /// Function to reset cart for next customer
    /// </summary>
    public void ResetCart()
    {
        // Track missed scanned items
        TrackMissedItems();

        // Destroy all spawned grocery items
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear(); // Clear the list for the next customer

        scannedItems.Clear();
        totalPrice = 0f;

        // Notify the current customer to exit
        if (currentCustomer != null)
        {
            GameObject currentNpc = currentCustomer.npcGameObject; // Assuming you keep track of the NPC GameObject in CustomerData
            if (currentNpc != null)
            {
                NPCController npcController = currentNpc.GetComponent<NPCController>();
                if (npcController != null)
                {
                    npcController.ExitStore(); // Transition to exiting state
                }
            }
        }

        currentCustomer = null; // Reset the current customer

        // Reset the UI
        worldSpaceUI.ResetUI();
        Debug.Log("Cart reset.");

        CheckoutTrigger checkoutTrigger = FindObjectOfType<CheckoutTrigger>();
        checkoutTrigger.CompleteTransaction();

        // Notify QueueManager to serve the next customer
        FindObjectOfType<QueueManager>().ServeNextCustomer();

    }

    /// <summary>
    /// Function to spawn items for customers
    /// </summary>
    /// <param name="customer"></param>
    public void SpawnItemsForCustomer(CustomerData customer)
    {
        if (currentCustomer == null) return;

        foreach (ShoppingItem shoppingItem in customer.shoppingList)
        {
            if (shoppingItem.itemPrefab != null)
            {
                // Instantiate the item at the counter
                GameObject item = Instantiate(
                    shoppingItem.itemPrefab,
                    spawnPoint.position + new Vector3(currentOffset, 0, 0),
                    Quaternion.identity
                );

                // Add the item to the spawnedItems list
                spawnedItems.Add(item);

                // Assign the item properties dynamically
                BarcodeItem barcodeItem = item.GetComponent<BarcodeItem>();
                if (barcodeItem != null)
                {
                    barcodeItem.itemName = shoppingItem.itemName;
                    barcodeItem.itemPrice = shoppingItem.itemPrice;
                }

                // Adjust the offset for the next item
                currentOffset += itemSpacing;
            }
        }

        // Reset the offset for the next customer
        currentOffset = 0f;
    }

    /// <summary>
    /// Function to void scanned item from checkout
    /// </summary>
    /// <param name="item"></param>
    public void VoidItem(BarcodeItem item)
    {
        if (scannedItems.Contains(item))
        {
            // Track overcharge mistake if the item is voided
            shiftDataTracker.TrackMistake("CustomerOvercharged");

            scannedItems.Remove(item);
            totalPrice -= item.itemPrice;

            // Update UI for voided items
            worldSpaceUI.RemoveScannedItem(item.itemName, item.itemPrice);

            // Update the displayed total price
            worldSpaceUI.UpdateTotalPrice(totalPrice);

            Debug.Log($"Item voided: {item.itemName}, Updated Total Price: ${totalPrice:F2}");
        }
        else
        {
            Debug.Log("Item not found in scanned list.");
        }
    }

    /// <summary>
    /// Function to track missed items
    /// </summary>
    private void TrackMissedItems()
    {
        if (currentCustomer == null) return;

        // Compare the ShoppingList to the scannedItems
        int missedItems = currentCustomer.shoppingList.Count - scannedItems.Count;

        if (missedItems > 0)
        {
            // Track the undercharge mistake
            shiftDataTracker.TrackMistake("CustomerUndercharged");
            Debug.Log($"Missed scanning {missedItems} items for customer {currentCustomer.fullName}.");
        }
    }

    /// <summary>
    /// Function to reject customer
    /// </summary>
    public void RejectCustomer()
    {
        if (currentCustomer != null)
        {
            if (currentCustomer.isFake ||
                (currentCustomer.isUnderage && scannedItems.Any(item => item.isRestricted)))
            {
                // Valid rejection
                Debug.Log($"Customer {currentCustomer.fullName} rejected for valid reasons (Fake ID or underage).");
            }
            else
            {
                // Invalid rejection
                shiftDataTracker.TrackMistake("InvalidRejection");
                Debug.Log($"Customer {currentCustomer.fullName} rejected without valid reason.");
            }

            // Reset for the next customer
            ResetCart();
        }
        else
        {
            Debug.LogWarning("No customer to reject.");
        }
    }
}
