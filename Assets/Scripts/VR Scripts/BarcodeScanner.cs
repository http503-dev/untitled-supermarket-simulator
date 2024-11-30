/*
 * Author: Muhammad Farhan
 * Date: 27/11/2024
 * Description: Script for handheld barcode scanner
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class BarcodeScanner : MonoBehaviour
{
    /// <summary>
    /// Settings for the barcode scanner
    /// </summary>
    public Transform rayOrigin; // The origin point of the scanner's ray (e.g., the scanner's tip)
    public float scanRange = 2f; // Maximum scanning range
    public string barcodeTag = "Scannable"; // Tag for items that can be scanned
    public AudioClip scanSound; // Sound effect for successful scans
    private AudioSource audioSource;
    private LineRenderer lineRenderer;

    private bool isBeingHeld = false; // Flag to track if the scanner is being held

    /// <summary>
    /// Setting the audio source and line renderer on awake
    /// </summary>
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // LineRenderer settings
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Basic material
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    /// <summary>
    /// Updating the line renderer live by calling the function 
    /// </summary>
    void Update()
    {
        UpdateLineRenderer();
    }

    /// <summary>
    /// Function to scan the barcode/grocery items
    /// </summary>
    public void Scan()
    {
        RaycastHit hit;

        // Cast a ray from the scanner
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, scanRange))
        {
            // Check if the object has the correct tag
            if (hit.collider.CompareTag(barcodeTag))
            {
                // Check if the object has a BarcodeItem component
                BarcodeItem barcodeItem = hit.collider.GetComponent<BarcodeItem>();
                if (barcodeItem != null)
                {
                    Debug.Log($"Scanned: {barcodeItem.itemName} - ${barcodeItem.itemPrice:F2}");

                    // Play the scan sound
                    if (scanSound != null)
                    {
                        audioSource.PlayOneShot(scanSound);
                    }

                    // Add the scanned item to the checkout list
                    CheckoutManager.Instance.AddItemToCart(barcodeItem);
                }
                else
                {
                    Debug.Log("No BarcodeItem component found on this object.");
                }
            }
            else
            {
                Debug.Log("Object is not tagged as Scannable.");
            }
        }
        else
        {
            Debug.Log("Nothing to scan.");
        }
    }

    /// <summary>
    /// Function to update line render 
    /// </summary>
    void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            // Update the positions of the line renderer to match the ray
            lineRenderer.SetPosition(0, rayOrigin.position);

            RaycastHit hit;
            if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, scanRange))
            {
                // If ray hits an object, draw up to the hit point
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.startColor = Color.red; // Change color when hitting an object
                lineRenderer.endColor = Color.red;
            }
            else
            {
                // If no hit, draw the full range
                lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * scanRange);
                lineRenderer.startColor = Color.green; // Default color
                lineRenderer.endColor = Color.green;
            }
        }
    }

    /// <summary>
    /// Called when the scanner is grabbed
    /// </summary>
    /// <param name="args"></param>
    public void OnGrabbed(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
        Debug.Log("Being held");
    }

    /// <summary>
    /// Called when the scanner is released
    /// </summary>
    /// <param name="args"></param>
    public void OnReleased(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        Debug.Log("Released");
    }
}
