using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public CustomerData Customer;

    public void Initialize(CustomerData customer)
    {
        Customer = customer;

        // Assign customer attributes to visual elements
        Debug.Log($"NPC Created: {Customer}");

        // Assign sprite index or other visual attributes
    }

    void Start()
    {
        // Add NPC-specific behaviors here
    }
}
