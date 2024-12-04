/*
 * Author: Muhammad Farhan
 * Date: 27/11/2024
 * Description: Script to set grocery info onto prefabs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarcodeItem : MonoBehaviour
{
    /// <summary>
    /// Info to place onto prefab
    /// </summary>
    public string itemName; // Name of the item
    public float itemPrice; // Price of the item
    public bool isRestricted; // Check whether item is 18+
}
