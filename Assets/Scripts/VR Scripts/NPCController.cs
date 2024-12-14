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

    private Animator animator;

    /// <summary>
    /// Assign the npc as nav mesh agent
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        // Check if the NPC is walking
        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
        SetWalking(isMoving);
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
                    StartWalkingTo(waypoints[0]); // Move to Entrance
                    yield return WaitForArrival();
                    // Wait a couple of seconds after arriving
                    //yield return new WaitForSeconds(4f); // 2 seconds delay
                    currentState = NPCState.Shopping;
                    break;

                case NPCState.Shopping:
                    StartWalkingTo(waypoints[1]); // Move to Aisles
                    yield return WaitForArrival();
                    yield return new WaitForSeconds(shoppingTime); // Simulate shopping
                    currentState = NPCState.Queueing;
                    break;

                case NPCState.Queueing:
                    if (!inQueue)
                    {
                        inQueue = true;
                        StartWalkingTo(waypoints[2]); // Move to Queue area
                        yield return WaitForArrival();
                        FindObjectOfType<QueueManager>().AddToQueue(this); // Add to queue
                    }

                    // Idle in queue
                    yield return new WaitForSeconds(0.5f);
                    break;

                case NPCState.Checkout:
                    StartWalkingTo(waypoints[3]); // Move to Checkout
                    yield return WaitForArrival();

                    // CheckoutTrigger will handle interaction
                    yield break;

                case NPCState.Exiting:
                    FindObjectOfType<NPCSpawner>().HandleNPCExit(this);
                    Debug.Log("NPC exited and handling npc exit");
                    StartWalkingTo(waypoints[4]); // Move to Exit
                    yield return WaitForArrival();
                    Destroy(gameObject); // Remove NPC
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
        FindObjectOfType<NPCSpawner>().HandleNPCExit(this);
        Debug.Log($"{CustomerData.fullName} is exiting the store.");

        StartWalkingTo(waypoints[4]);
    }

    /// <summary>
    /// Function to move npc from one place to another
    /// </summary>
    /// <param name="destination"></param>
    private void StartWalkingTo(Transform destination)
    {
        if (animator != null)
            animator.SetBool("isWalking", true); // Start walking animation

        target = destination;
        agent.SetDestination(target.position);
    }

    public event System.Action<GameObject> OnNPCExit;

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

    /// <summary>
    /// Function to enable or disable walking animation
    /// </summary>
    /// <param name="isWalking"></param>
    private void SetWalking(bool isWalking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }
}
