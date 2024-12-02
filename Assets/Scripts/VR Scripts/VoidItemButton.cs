/*
 * Author: Jarene Goh
 * Date: 1/12/2024
 * Description: Script to handle the button for voiding items
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidItemButton : MonoBehaviour
{
    private BarcodeItem linkedItem;

    /// <summary>
    /// Function to assign the item to this button
    /// </summary>
    /// <param name="item"></param>
    public void Initialize(BarcodeItem item)
    {
        linkedItem = item;
    }

    /// <summary>
    /// Called when the button is clicked
    /// </summary>
    public void VoidItem()
    {
        if (linkedItem != null)
        {
            CheckoutManager.Instance.VoidItem(linkedItem);
        }
        else
        {
            Debug.LogWarning("VoidItemButton: No BarcodeItem linked to this button.");
        }
    }
}
