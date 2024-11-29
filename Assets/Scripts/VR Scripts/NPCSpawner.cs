using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // Assign NPC prefab here
    public Transform spawnPoint; // Where NPCs will spawn

    public void SpawnNPC(CustomerData customer)
    {
        // Instantiate the NPC at the spawn point
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);

        // Initialize the NPC with customer data
        NPCController npcController = npc.GetComponent<NPCController>();
        if (npcController != null)
        {
            npcController.Initialize(customer);
        }
    }

    void Start()
    {
        // Spawn 3 random NPCs at the start
        for (int i = 0; i < 3; i++)
        {
            CustomerData customer = CustomerGenerator.GenerateCustomer();
            SpawnNPC(customer);
        }
    }
}
