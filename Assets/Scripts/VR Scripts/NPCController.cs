/*
 * Author: Muhammad Farhan
 * Date: 26/11/2024
 * Description: Function to intialize customers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    /// <summary>
    /// Reference to customer data class
    /// </summary>
    public CustomerData CustomerData;
    public Image mugshotImage; // Reference to the UI Image component

    /// <summary>
    /// References to things needed for npc behaviours
    /// </summary>
    public enum NPCState { Entering, Shopping, Queueing, Checkout, Exiting }
    public NPCState currentState = NPCState.Entering;

    private NavMeshAgent agent;
    private Transform target;
    private float shoppingTime = 5f; // Time spent shopping
    private bool inQueue = false;

    public Transform[] waypoints; // Assign waypoints: Entrance, Aisles, Queue, Checkout, Exit

    /// <summary>
    /// Assign the npc as nav mesh agent
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// Function to initialize a customer
    /// </summary>
    /// <param name="customer"></param>
    public void Initialize(CustomerData customer)
    {
        CustomerData = customer;

        // Assign customer attributes to visual elements
        Debug.Log($"NPC Created: {CustomerData}");

        // Update mugshot image
        if (mugshotImage != null)
        {
            mugshotImage.sprite = FindObjectOfType<CustomerGenerator>().mugshotSprites[customer.spriteIndex];
        }

        // Start entering the supermarket
        StartCoroutine(HandleState());
    }

    /// <summary>
    /// Coroutines to handle npc behaviour
    /// </summary>
    /// <returns></returns>
    private IEnumerator HandleState()
    {
        while (true)
        {
            switch (currentState)
            {
                case NPCState.Entering:
                    MoveTo(waypoints[0]); // Entrance
                    yield return WaitForArrival();
                    currentState = NPCState.Shopping;
                    break;

                case NPCState.Shopping:
                    // Walk through shopping aisles
                    MoveTo(waypoints[1]); // Aisles
                    yield return WaitForArrival();
                    yield return new WaitForSeconds(shoppingTime); // Simulate shopping
                    currentState = NPCState.Queueing;
                    break;

                case NPCState.Queueing:
                    if (!inQueue)
                    {
                        inQueue = true;
                        MoveTo(waypoints[2]); // Queue area
                        yield return WaitForArrival();
                        FindObjectOfType<QueueManager>().AddToQueue(this);
                    }

                    // Wait in queue until assigned to checkout
                    yield return new WaitForSeconds(0.5f);
                    break;

                case NPCState.Checkout:
                    MoveTo(waypoints[3]); // Checkout
                    yield return WaitForArrival();

                    // CheckoutTrigger will handle interaction
                    yield break;

                case NPCState.Exiting:
                    MoveTo(waypoints[4]); // Exit
                    yield return WaitForArrival();

                    // Notify the spawner and destroy the NPC
                    OnNPCExit?.Invoke(gameObject);
                    Destroy(gameObject); // Remove NPC from scene
                    yield break;
            }
        }
    }

    /// <summary>
    /// Function to allow the start checkout
    /// </summary>
    public void StartCheckout()
    {
        currentState = NPCState.Checkout;
        inQueue = false; // Allow the next NPC to queue
    }

    /// <summary>
    /// Function to make the npc leave the store
    /// </summary>
    public void ExitStore()
    {
        currentState = NPCState.Exiting; // Set state to Exiting
        Debug.Log($"{CustomerData.fullName} is exiting the store.");

        // Move to the exit waypoint
        MoveTo(waypoints[4]);
    }

    public event System.Action<GameObject> OnNPCExit;

    /// <summary>
    /// Function to move npc from one place to another
    /// </summary>
    /// <param name="destination"></param>
    private void MoveTo(Transform destination)
    {
        target = destination;
        agent.SetDestination(target.position);
    }

    /// <summary>
    /// Coroutine to stop npcs
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForArrival()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
    }
}
