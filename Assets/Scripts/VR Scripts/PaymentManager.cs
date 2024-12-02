using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaymentManager : MonoBehaviour
{
    public CheckoutUI checkoutUI; // Reference to the world space UI
    private CheckoutManager checkoutManager; // Reference to CheckoutManager
    public float customerPayment = 0f; // The amount paid by the customer
    private bool registerClosed = false;


    void Start()
    {
        // Get the CheckoutManager reference
        checkoutManager = CheckoutManager.Instance;
    }

    public void StartPayment()
    {
        if (checkoutManager.currentCustomer == null) return;

        // Generate a random payment amount
        customerPayment = checkoutManager.currentCustomer.TotalPrice + Random.Range(-1f, 10f);
        customerPayment = Mathf.Max(0, customerPayment); // Ensure payment is not negative

        Debug.Log($"Customer hands over: ${customerPayment:F2}");

        // Display the payment in the UI
        checkoutUI.UpdatePayment(customerPayment);

        // Handle payment based on whether it's sufficient or not
        if (customerPayment >= checkoutManager.currentCustomer.TotalPrice)
        {
            float change = customerPayment - checkoutManager.currentCustomer.TotalPrice;

            // Display change
            Debug.Log($"Payment sufficient. Change to give: ${change:F2}");
            checkoutUI.DisplayChange(change);

            // Open the cash register but do not complete the transaction yet
            StartCoroutine(OpenCashRegister(change));
        }
        else
        {
            HandleUnderpayment();
        }
    }

    private void HandleUnderpayment()
    {
        float amountNeeded = checkoutManager.currentCustomer.TotalPrice - customerPayment;
        Debug.Log($"Underpayment. Customer still owes: ${amountNeeded:F2}");

        // Display underpayment message in the UI
        checkoutUI.DisplayUnderpayment(amountNeeded);

        // Allow the player to request more cash from the customer
        checkoutUI.ShowRequestMoreCashButton();
    }

    public void RequestMoreCash()
    {
        if (checkoutManager.currentCustomer == null) return;

        // Generate additional cash from the customer
        float additionalCash = Random.Range(1f, 5f); // Customer hands over 1 to 5 dollars more
        customerPayment += additionalCash;

        Debug.Log($"Customer hands over additional cash: ${additionalCash:F2}");
        checkoutUI.UpdatePayment(customerPayment);

        // Check if the new payment is sufficient
        if (customerPayment >= checkoutManager.currentCustomer.TotalPrice)
        {
            float change = customerPayment - checkoutManager.currentCustomer.TotalPrice;

            // Display change
            Debug.Log($"Payment sufficient. Change to give: ${change:F2}");
            checkoutUI.DisplayChange(change);

            // Open the cash register but wait for player interaction to complete transaction
            StartCoroutine(OpenCashRegister(change));
        }
        else
        {
            float amountNeeded = checkoutManager.currentCustomer.TotalPrice - customerPayment;
            Debug.Log($"Still underpaid. Customer owes: ${amountNeeded:F2}");
            checkoutUI.DisplayUnderpayment(amountNeeded);
        }
    }
    private void CompleteTransaction()
    {
        Debug.Log("Transaction complete. Ready for the next customer.");

        // Reset the checkout for the next customer
        checkoutManager.ResetCart();
        registerClosed = false; // Reset the register closed state
    }

    private IEnumerator OpenCashRegister(float change)
    {
        // Trigger cash register open animation
        Debug.Log("Cash register opens.");
        checkoutUI.DisplayChange(change);

        // Wait for the player to close the cash register
        while (!registerClosed)
        {
            // Example: Replace with VR interaction or a button press
            if (Input.GetKeyDown(KeyCode.C)) // Player closes the register (replace with VR input)
            {
                registerClosed = true;
            }
            yield return null;
        }

        // Reset transaction after the register is closed
        CompleteTransaction();
    }

    public void CloseRegister()
    {
        registerClosed = true;
        Debug.Log("Register closed by the player.");
        CompleteTransaction();
    }

}
