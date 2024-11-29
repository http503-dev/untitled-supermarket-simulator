using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    // List of possible first names
    private static readonly List<string> FirstNames = new List<string> {
        "Alice", "Bob", "Charlie", "Diana", "Eve", "Frank", "Grace", "Hank", "Ivy", "Jack"
    };

    // List of possible last names
    private static readonly List<string> LastNames = new List<string> {
        "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Lee", "Garcia", "Martinez", "Clark", "Walker"
    };

    // List of grocery items with prices
    private static readonly List<(string Name, float Price)> Items = new List<(string, float)> {
        ("Canned Tuna", 3.55f), ("Milk", 5.50f), ("Bread", 3.00f), ("Eggs", 4.50f),
        ("Rice", 6.00f), ("Pasta", 2.00f), ("Flour", 2.50f), ("Sugar", 3.00f),
        ("Water", 1.50f), ("Bottled Drinks", 2.50f), ("Chips", 2.50f),
        ("Coffee", 7.00f), ("Tea", 6.50f)
    };

    // Generate a random customer
    public static CustomerData GenerateCustomer()
    {
        var random = new System.Random();

        string firstName = FirstNames[random.Next(FirstNames.Count)];
        string lastName = LastNames[random.Next(LastNames.Count)];
        DateTime dateOfBirth = GenerateRandomDate(new DateTime(1960, 1, 1), new DateTime(2005, 12, 31));
        int spriteIndex = random.Next(1, 11); // Assume 10 mugshots available
        DateTime expiryDate = GenerateRandomDate(new DateTime(2023, 1, 1), new DateTime(2030, 12, 31));
        bool isBlacklisted = random.Next(0, 2) == 1;

        // Assign groceries
        List<(string Name, float Price)> groceries = AssignGroceries(random);
        float totalPrice = CalculateTotalPrice(groceries);

        return new CustomerData
        {
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            SpriteIndex = spriteIndex,
            ExpiryDate = expiryDate,
            IsBlacklisted = isBlacklisted,
            Groceries = groceries,
            TotalPrice = totalPrice
        };
    }

    // Generate a list of groceries
    private static List<(string Name, float Price)> AssignGroceries(System.Random random)
    {
        int itemCount = random.Next(1, 10); // Random number of items (1 to 10)
        var selectedItems = new List<(string Name, float Price)>();
        var availableItems = new List<(string Name, float Price)>(Items);

        for (int i = 0; i < itemCount; i++)
        {
            int randomIndex = random.Next(availableItems.Count);
            selectedItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex); // Avoid duplicates
        }

        return selectedItems;
    }

    // Calculate total price of selected items
    private static float CalculateTotalPrice(List<(string Name, float Price)> groceries)
    {
        float total = 0;
        foreach (var item in groceries)
        {
            total += item.Price;
        }
        return total;
    }

    // Generate a random date between two dates
    private static DateTime GenerateRandomDate(DateTime startDate, DateTime endDate)
    {
        var random = new System.Random();
        int range = (endDate - startDate).Days;
        return startDate.AddDays(random.Next(range));
    }
}
