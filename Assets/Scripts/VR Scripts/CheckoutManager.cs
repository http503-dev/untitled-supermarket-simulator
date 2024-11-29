using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutManager : MonoBehaviour
{
    public static CheckoutManager Instance; // Singleton pattern

    private List<BarcodeItem> scannedItems = new List<BarcodeItem>();
    public float totalPrice = 0f;

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

    public void AddItemToCart(BarcodeItem item)
    {
        scannedItems.Add(item);
        totalPrice += item.itemPrice;
        Debug.Log($"Item added: {item.itemName}, Total Price: ${totalPrice:F2}");
    }

    public void ResetCart()
    {
        scannedItems.Clear();
        totalPrice = 0f;
        Debug.Log("Cart reset.");
    }
}
