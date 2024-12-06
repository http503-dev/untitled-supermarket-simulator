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
    public string firstName;
    public string lastName;

    public List<ShoppingItem> shoppingList = new List<ShoppingItem>();

    public string fullName => $"{firstName} {lastName}";
    public DateTime dateOfBirth;
    public int spriteIndex; // Mugshot index
    public List<(string Name, float Price)> Groceries;
    public float totalPrice;


    public const int legalAge = 18; // Minimum age for purchasing restricted items

    public bool isUnderage
    {
        get
        {
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear) age--;
            return age < legalAge;
        }
    }

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
                       $"Full Name: {fullName}\n" +
                       $"Date of Birth: {dateOfBirth.ToShortDateString()}\n" +
                       $"Sprite Index: {spriteIndex}\n" +
                       $"Total Price: ${totalPrice:F2}\n" +
                       $"Underage: {isUnderage}\n" +
                       $"Shopping List:";

        foreach (var item in shoppingList)
        {
            stats += $"\n - {item.itemName}: ${item.itemPrice:F2}";
        }

        return stats;
    }
}
