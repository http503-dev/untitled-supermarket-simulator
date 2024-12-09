/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to spawn customers
 */
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
public class NPCSpawner : MonoBehaviour
{
    /// <summary>
    /// References to prefabs, spawn point and customer generator script
    /// </summary>
    public Transform spawnPoint; // Where NPCs will spawn
    public CustomerGenerator customerGenerator; // Reference to Customer Generator
    private DatabaseReference databaseReference; // Firebase reference
    private FirebaseAuth auth;
    private FirebaseUser user;

    /// <summary>
    /// Function to call the SpawnNPC function on start (and to set how many to spawn)
    /// </summary>
    void Start()
    {
        // Initialize Firebase reference
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        // Spawn 1 random NPCs at the start
        for (int i = 0; i < 1; i++)
        {
            // Generate customer data with the corresponding prefab
            var (customer, customerPrefab) = customerGenerator.GenerateCustomer();
            SpawnNPC(customer, customerPrefab);

            // Log the customer's stats
            Debug.Log(customer.GetCustomerStats());
        }
    }

    /// <summary>
    /// Function to spawn customer 
    /// </summary>
    /// <param name="customer"></param>
    public void SpawnNPC(CustomerData customer, GameObject customerPrefab)
    {
        // Instantiate the NPC at the spawn point
        GameObject npc = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Initialize the NPC with customer data
        NPCController npcController = npc.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.Initialize(customer);
        }
    }

    /// <summary>
    /// Function to send customer data to database
    /// </summary>
    /// <param name="customer"></param>
    public void SaveCustomerToDatabase(CustomerData customer)
    {
        // Convert CustomerData to a Dictionary for Firebase
        Dictionary<string, object> customerData = new Dictionary<string, object>
        {
            { "fullName", customer.fullName },
            { "dateOfBirth", customer.dateOfBirth.ToString("yyyy-MM-dd") },
            { "spriteIndex", customer.spriteIndex },
            { "totalPrice", customer.totalPrice },
            { "isUnderage", customer.isUnderage },
            { "isFake", customer.isFake }
        };

        // Add the shopping list to the customer data
        List<Dictionary<string, object>> shoppingListData = new List<Dictionary<string, object>>();
        foreach (var item in customer.shoppingList)
        {
            shoppingListData.Add(new Dictionary<string, object>
            {
                { "itemName", item.itemName },
                { "itemPrice", item.itemPrice }
            });
        }
        customerData["shoppingList"] = shoppingListData;

        // Push the customer data to Firebase
        databaseReference.Child("sessions").Child(user.UserId).Child("customer").SetValueAsync(customerData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Customer data saved to database: {customer.fullName}");
            }
            else
            {
                Debug.LogError($"Failed to save customer data: {task.Exception}");
            }
        });
    }
}
