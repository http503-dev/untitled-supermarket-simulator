/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to generate customers
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CustomerData;

public class CustomerGenerator : MonoBehaviour
{
    /// <summary>
    /// List of possible first names
    /// </summary>
    private static readonly List<string> FirstNames = new List<string> {
        "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack"
    };

    /// <summary>
    /// List of possible last names
    /// </summary>
    private static readonly List<string> LastNames = new List<string> {
        "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Lee", "Garcia", "Martinez", "Clark", "Walker"
    };

    /// <summary>
    /// Prefabs for all available items
    /// </summary>
    public List<GameObject> itemPrefabs; 

    /// <summary>
    /// Function to generate a random customer
    /// </summary>
    /// <returns></returns>
    public CustomerData GenerateCustomer()
    {
        var random = new System.Random();

        string firstName = FirstNames[random.Next(FirstNames.Count)];
        string lastName = LastNames[random.Next(LastNames.Count)];
        DateTime dateOfBirth = GenerateRandomDate(new DateTime(1960, 1, 1), new DateTime(2005, 12, 31));
        int spriteIndex = random.Next(1, 11); // Assume 10 mugshots available
        DateTime expiryDate = GenerateRandomDate(new DateTime(2023, 1, 1), new DateTime(2030, 12, 31));
        bool isBlacklisted = random.Next(0, 2) == 1;

        // Generate shopping list
        List<ShoppingItem> shoppingList = GenerateShoppingList();

        // Calculate total price
        float totalPrice = 0f;
        foreach (var item in shoppingList)
        {
            totalPrice += item.itemPrice;
        }


        return new CustomerData
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            SpriteIndex = spriteIndex,
            ExpiryDate = expiryDate,
            IsBlacklisted = isBlacklisted,
            ShoppingList = shoppingList,
            TotalPrice = totalPrice
        };
    }

    /// <summary>
    /// Function to generate a random shopping list based on available prefabs
    /// </summary>
    /// <returns></returns>
    private List<ShoppingItem> GenerateShoppingList()
    {
        var shoppingList = new List<ShoppingItem>();
        var random = new System.Random();

        int numberOfItems = random.Next(1, 6); // Randomly pick 1–5 items

        for (int i = 0; i < numberOfItems; i++)
        {
            int randomIndex = random.Next(itemPrefabs.Count);
            GameObject prefab = itemPrefabs[randomIndex];

            BarcodeItem barcodeItem = prefab.GetComponent<BarcodeItem>();
            if (barcodeItem != null)
            {
                shoppingList.Add(new ShoppingItem
                {
                    itemName = barcodeItem.itemName,
                    itemPrice = barcodeItem.itemPrice,
                    itemPrefab = prefab
                });
            }
        }

        return shoppingList;
    }

    /// <summary>
    /// Function to generate a random date between 2 dates (for id stuff)
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    private static DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
    {
        var random = new System.Random();
        int range = (endDate - startDate).Days;
        return startDate.AddDays(random.Next(range));
    }
}
