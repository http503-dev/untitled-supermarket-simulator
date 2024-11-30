/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Classes for customer data and shopping items
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerData
{
    /// <summary>
    /// Customer 'stats'
    /// </summary>
    public string FirstName;
    public string LastName;

    public List<ShoppingItem> ShoppingList = new List<ShoppingItem>();

    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth;
    public int SpriteIndex; // Mugshot index
    public DateTime ExpiryDate;
    public bool IsBlacklisted;
    public List<(string Name, float Price)> Groceries;
    public float TotalPrice;

    /// <summary>
    /// Customer's shopping list
    /// </summary>
    [System.Serializable]
    public class ShoppingItem
    {
        public string itemName;
        public float itemPrice;
        public GameObject itemPrefab; // Reference to the prefab of the item
    }

    /// <summary>
    /// Function to format customer stats
    /// </summary>
    /// <returns></returns>
    public string GetCustomerStats()
    {
        string stats = $"Customer Stats:\n" +
                       $"Full Name: {FullName}\n" +
                       $"Date of Birth: {DateOfBirth.ToShortDateString()}\n" +
                       $"Sprite Index: {SpriteIndex}\n" +
                       $"ID Expiry Date: {ExpiryDate.ToShortDateString()}\n" +
                       $"Blacklisted: {IsBlacklisted}\n" +
                       $"Total Price: ${TotalPrice:F2}\n" +
                       $"Shopping List:";

        foreach (var item in ShoppingList)
        {
            stats += $"\n - {item.itemName}: ${item.itemPrice:F2}";
        }

        return stats;
    }
}
