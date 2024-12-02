/*
 * Author: Jarene Goh
 * Date: 1/12/2024
 * Description: Script to handle the ui for checking out
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckoutUI : MonoBehaviour
{
    /// <summary>
    /// References to UI elements
    /// </summary>
    public TextMeshProUGUI totalPriceText;
    public TextMeshProUGUI paymentText;
    public TextMeshProUGUI changeText;
    public TextMeshProUGUI underpaymentText;
    public Transform scannedItemsList;

    /// <summary>
    /// A prefab for displaying scanned items in the UI
    /// </summary>
    public GameObject scannedItemPrefab;


    public GameObject requestMoreCashButton; // Button to request more cash

    /// <summary>
    /// Function to update total price text
    /// </summary>
    /// <param name="totalPrice"></param>
    public void UpdateTotalPrice(float totalPrice)
    {
        totalPriceText.text = $"Total Price: ${totalPrice:F2}";
    }

    /// <summary>
    /// Function to show how much customer pays
    /// </summary>
    /// <param name="payment"></param>
    public void UpdatePayment(float payment)
    {
        paymentText.text = $"Payment: ${payment:F2}";
    }

    /// <summary>
    /// Function to show how much change is to be given
    /// </summary>
    /// <param name="change"></param>
    public void DisplayChange(float change)
    {
        changeText.text = $"Change: ${change:F2}";
        underpaymentText.text = ""; // Clear underpayment message

        requestMoreCashButton.SetActive(false); // Hide the request button
    }

    /// <summary>
    /// Function to show how much more the customer need to give
    /// </summary>
    /// <param name="amountNeeded"></param>
    public void DisplayUnderpayment(float amountNeeded)
    {
        underpaymentText.text = $"Underpayment: ${amountNeeded:F2}";
        changeText.text = ""; // Clear change message

        requestMoreCashButton.SetActive(true); // Show the request button
    }

    /// <summary>
    /// Function to update scanned items to UI
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="itemPrice"></param>
    /// <param name="itemReference"></param>
    public void AddScannedItem(string itemName, float itemPrice, BarcodeItem itemReference)
    {
        // Instantiate a new UI element for the scanned item
        GameObject itemUI = Instantiate(scannedItemPrefab, scannedItemsList);

        // Set the item's text to display name and price
        TextMeshProUGUI itemText = itemUI.GetComponentInChildren<TextMeshProUGUI>();
        if (itemText != null)
        {
            itemText.text = $"{itemName}: ${itemPrice:F2}";
        }

        // Assign the itemReference to the button for void functionality
        VoidItemButton voidItemButton = itemUI.GetComponentInChildren<VoidItemButton>();
        if (voidItemButton != null)
        {
            voidItemButton.Initialize(itemReference);
        }
    }

    /// <summary>
    /// Function to remove scanned item UI
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="itemPrice"></param>
    public void RemoveScannedItem(string itemName, float itemPrice)
    {
        foreach (Transform child in scannedItemsList)
        {
            TextMeshProUGUI itemText = child.GetComponentInChildren<TextMeshProUGUI>();
            if (itemText != null && itemText.text.Contains(itemName))
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    /// <summary>
    /// Function to reset UI
    /// </summary>
    public void ResetUI()
    {
        totalPriceText.text = "Total Price: $0.00";
        paymentText.text = "Payment: $0.00";
        changeText.text = "";
        underpaymentText.text = "";

        foreach (Transform child in scannedItemsList)
        {
            Destroy(child.gameObject);
        }
    }


    public void ShowRequestMoreCashButton()
    {
        requestMoreCashButton.SetActive(true);
    }

    public void HideRequestMoreCashButton()
    {
        requestMoreCashButton.SetActive(false);
    }
}
