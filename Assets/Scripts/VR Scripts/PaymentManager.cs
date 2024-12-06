/*
 * Author: Alfred Kang
 * Date: 2/12/2024
 * Description: Script to handle payment
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    /// <summary>
    /// References to things in the scene
    /// </summary>
    public CheckoutUI checkoutUI; // Reference to the UI script
    private CheckoutManager checkoutManager; // Reference to CheckoutManager
    public GameObject cashRegister; // Cash register GameObject
    public Transform cashSpawnPoint; // Position to spawn change objects
    public ShiftDataTracker shiftDataTracker; // Reference to ShiftDataTracker

    /// <summary>
    /// class/list to store the cash of different denominations
    /// </summary>
    [System.Serializable]
    public class CashPrefabEntry
    {
        public float denomination; // The denomination of the cash (e.g., 1.0 for $1 bill)
        public GameObject cashPrefab; // The prefab for this denomination
    }
    public List<CashPrefabEntry> cashPrefabMap; // List of cash prefabs mapped to their denominations

    /// <summary>
    /// Floats/bools to check various things 
    /// </summary>
    private float customerPayment = 0f; // Customer's total payment
    private bool registerClosed = false; // Bool to check if register is closed

    private float change = 0f; // The total change to be given to the player
    private List<float> grabbedCash = new List<float>(); // Track the denominations grabbed by the player

    /// <summary>
    /// Denominations we are working with
    /// </summary>
    public readonly float[] denominations = { 20.0f, 10.0f, 5.0f, 2.0f, 1.0f, 0.50f, 0.20f, 0.10f, 0.05f, 0.01f };

    /// <summary>
    /// Get the CheckoutManager reference
    /// </summary>
    void Start()
    {
        checkoutManager = CheckoutManager.Instance;
    }

    /// <summary>
    /// Activate the cash register and generate customer cash offer
    /// </summary>
    public void ActivateCashRegister()
    {
        cashRegister.SetActive(true); // Open the cash register
        GenerateCustomerCash();
    }

    /// <summary>
    /// Function to generate an amount for the customer to give/pay
    /// </summary>
    private void GenerateCustomerCash()
    {
        if (checkoutManager.currentCustomer == null) return;

        float scannedTotal = checkoutManager.totalPrice;

        // Customer offers cash within a range, based on scanned total
        customerPayment = scannedTotal + Random.Range(-1f, 5f); // Adjust range as needed
        customerPayment = Mathf.Max(0, customerPayment); // Ensure no negative cash is offered

        Debug.Log($"Customer offers: ${customerPayment:F2}");
        checkoutUI.UpdatePayment(customerPayment);

        // Handle payment scenarios
        if (customerPayment >= scannedTotal)
        {
            HandleChange();
        }
        else
        {
            HandleUnderpayment();
        }
    }

    /// <summary>
    /// Handle underpayment
    /// </summary>
    private void HandleUnderpayment()
    {
        float amountNeeded = checkoutManager.totalPrice - customerPayment;
        Debug.Log($"Underpayment detected. Customer owes: ${amountNeeded:F2}");
        checkoutUI.DisplayUnderpayment(amountNeeded);
        checkoutUI.ShowRequestMoreCashButton();
    }

    /// <summary>
    /// Request additional cash from the customer
    /// </summary>
    public void RequestMoreCash()
    {
        if (checkoutManager.currentCustomer == null) return;

        float additionalCash = Random.Range(1f, 10f); // Adjust range as needed
        customerPayment += additionalCash;

        Debug.Log($"Customer hands over additional cash: ${additionalCash:F2}");
        checkoutUI.UpdatePayment(customerPayment);

        if (customerPayment >= checkoutManager.totalPrice)
        {
            HandleChange();
        }
        else
        {
            HandleUnderpayment();
        }
    }

    /// <summary>
    /// Handle overpayment and calculate change
    /// </summary>
    private void HandleChange()
    {
        change = customerPayment - checkoutManager.totalPrice;
        Debug.Log($"Overpayment detected. Change to give: ${change:F2}");
        checkoutUI.DisplayChange(change);
    }

    /// <summary>
    /// Function to grab cash out of the register for handling change
    /// </summary>
    /// <param name="denomination"></param>
    public void GrabCash(float denomination)
    {
        // The player grabs a denomination from the register
        grabbedCash.Add(denomination);

        // Find the prefab corresponding to the grabbed denomination
        CashPrefabEntry cashPrefabEntry = cashPrefabMap.Find(entry => entry.denomination == denomination);

        if (cashPrefabEntry != null)
        {
            // Instantiate the cash prefab at the spawn point
            GameObject cashObject = Instantiate(cashPrefabEntry.cashPrefab, cashSpawnPoint.position, Quaternion.identity);

            // Optionally, you can add some offset to stack the cash objects or animate them
            cashObject.transform.position += new Vector3(grabbedCash.Count * 0.1f, 0, 0); // Adjust for stacking

            Debug.Log($"Cash object for ${denomination} instantiated.");
        }
        else
        {
            Debug.LogWarning($"No prefab found for denomination ${denomination}.");
        }
    }

    /// <summary>
    /// Close the cash register and complete the transaction
    /// </summary>
    public void CloseRegister()
    {
        // Complete the transaction, regardless of whether the cash is correct
        Debug.Log("Cash register closed. Transaction complete.");

        // logging the mistakes or incorrect change
        float totalGrabbed = 0f;

        foreach (float cash in grabbedCash)
        {
            totalGrabbed += cash;

            if (totalGrabbed > change)
            {
                shiftDataTracker.TrackMistake("ExcessChange");
            }
            else if (totalGrabbed < change)
            {
                shiftDataTracker.TrackMistake("InsufficientChange");
            }
        }

        // Check if the customer payment was insufficient but the player ignored it
        if (customerPayment < checkoutManager.totalPrice)
        {
            shiftDataTracker.TrackMistake("InsufficientPayment");
            Debug.Log("Insufficient payment mistake: Player did not request enough cash.");
        }

        // Check for restricted sales
        if (checkoutManager.currentCustomer != null && checkoutManager.currentCustomer.isUnderage)
        {
            foreach (var item in checkoutManager.scannedItems)
            {
                if (item.isRestricted)
                {
                    shiftDataTracker.TrackMistake("RestrictedSale");
                    Debug.Log("Restricted sale detected.");
                }
            }
        }

        Debug.Log($"Player grabbed a total of: ${totalGrabbed:F2}");

        // Reset the state for the next transaction
        cashRegister.SetActive(false);
        checkoutManager.ResetCart(); // Reset for next customer
        checkoutUI.ResetUI();
        customerPayment = 0f;
        grabbedCash.Clear(); // Clear the grabbed cash list
    }
}
