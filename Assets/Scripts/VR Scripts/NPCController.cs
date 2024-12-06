/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to intialize customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    /// <summary>
    /// Reference to customer data class
    /// </summary>
    public CustomerData CustomerData;
    public Image mugshotImage; // Reference to the UI Image component

    /// <summary>
    /// Function to initialize a customer
    /// </summary>
    /// <param name="customer"></param>
    public void Initialize(CustomerData customer)
    {
        CustomerData = customer;

        // Assign customer attributes to visual elements
        Debug.Log($"NPC Created: {CustomerData}");

        // Update mugshot image
        if (mugshotImage != null)
        {
            mugshotImage.sprite = FindObjectOfType<CustomerGenerator>().mugshotSprites[customer.spriteIndex];
        }
    }
}
