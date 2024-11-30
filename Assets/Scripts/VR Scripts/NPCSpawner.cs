/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to spawn customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    /// <summary>
    /// References to prefabs, spawn point and customer generator script
    /// </summary>
    public GameObject npcPrefab; // Assign NPC prefab here
    public Transform spawnPoint; // Where NPCs will spawn
    public CustomerGenerator customerGenerator; // Reference to Customer Generator

    /// <summary>
    /// Function to spawn customer 
    /// </summary>
    /// <param name="customer"></param>
    public void SpawnNPC(CustomerData customer)
    {
        // Instantiate the NPC at the spawn point
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // Initialize the NPC with customer data
        NPCController npcController = npc.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.Initialize(customer);
        }
    }

    /// <summary>
    /// Function to call the SpawnNPC function on start (and to set how many to spawn)
    /// </summary>
    void Start()
    {
        // Spawn 1 random NPCs at the start
        for (int i = 0; i < 1; i++)
        {
            CustomerData customer = customerGenerator.GenerateCustomer();
            SpawnNPC(customer);
            // Log the customer's stats
            Debug.Log(customer.GetCustomerStats());
        }
    }
}
