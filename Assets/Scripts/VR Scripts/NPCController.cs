/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to intialize customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    /// <summary>
    /// Reference to customer data class
    /// </summary>
    public CustomerData CustomerData;

    /// <summary>
    /// Function to initialize a customer
    /// </summary>
    /// <param name="customer"></param>
    public void Initialize(CustomerData customer)
    {
        CustomerData = customer;

        // Assign customer attributes to visual elements
        Debug.Log($"NPC Created: {CustomerData}");

        // Assign sprite index or other visual attributes
    }

    void Start()
    {
        // Add NPC-specific behaviors here
    }
}
