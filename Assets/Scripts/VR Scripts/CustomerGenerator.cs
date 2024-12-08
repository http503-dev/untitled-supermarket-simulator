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
    private static readonly List<string> firstNames = new List<string> {
        "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack"
    };

    /// <summary>
    /// List of possible last names
    /// </summary>
    private static readonly List<string> lastNames = new List<string> {
        "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Lee", "Garcia", "Martinez", "Clark", "Walker"
    };

    /// <summary>
    /// Prefabs for all available items
    /// </summary>
    public List<GameObject> itemPrefabs;

    /// <summary>
    /// Array of mugshot for IDs
    /// </summary>
    public Sprite[] mugshotSprites;

    /// <summary>
    /// Function to generate a random customer
    /// </summary>
    /// <returns></returns>
    public CustomerData GenerateCustomer()
    {
        var random = new System.Random();

        string firstName = firstNames[random.Next(firstNames.Count)];
        string lastName = lastNames[random.Next(lastNames.Count)];
        DateTime dateOfBirth = GenerateRandomDate(new DateTime(1960, 1, 1), new DateTime(2005, 12, 31));
        int spriteIndex = random.Next(0, mugshotSprites.Length); // Get random mugshot index
        bool isFake = random.Next(0, 2) == 1;

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
            firstName = firstName,
            lastName = lastName,
            dateOfBirth = dateOfBirth,
            spriteIndex = spriteIndex,
            shoppingList = shoppingList,
            totalPrice = totalPrice,
            isFake = isFake
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

        int numberOfItems = random.Next(1, 4); // Randomly pick 1–3 items

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
                    itemPrice = Mathf.Round(barcodeItem.itemPrice * 100) / 100,
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
