using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerData
{
    public string FirstName;
    public string LastName;
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateOfBirth;
    public int SpriteIndex; // Mugshot index
    public DateTime ExpiryDate;
    public bool IsBlacklisted;
    public List<(string Name, float Price)> Groceries;
    public float TotalPrice;

    public override string ToString()
    {
        return $"{FullName}, DoB: {DateOfBirth.ToShortDateString()}, Sprite: {SpriteIndex}, " +
               $"Expiry: {ExpiryDate.ToShortDateString()}, Blacklisted: {IsBlacklisted}, " +
               $"Items: {Groceries.Count}, Total: ${TotalPrice:F2}";
    }
}
