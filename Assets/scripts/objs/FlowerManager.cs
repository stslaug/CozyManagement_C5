using DataModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles flower installation
//Instantiates, Removes, and Updates all flower instances 
public class FlowerManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to GameManager

    private void Update()
    {
        
    }
    // Add a new flower of the specified type at the specified position
    public GameObject SpawnFlower(Vector3 position, FlowerConfig flowerConfig, Inventory inventory)
    {
        if (flowerConfig == null || flowerConfig.prefab == null)
        {
            Debug.LogWarning("FlowerConfig or prefab is null. Cannot spawn flower.");
            return null;
        }

        Debug.Log($"Spawning flower: {flowerConfig.flowerType} at {position}");

        // Instantiate the flower prefab
        GameObject newFlower = Instantiate(flowerConfig.prefab, position, Quaternion.identity);
        if (newFlower == null)
        {
            Debug.LogError("Failed to instantiate flower prefab.");
            return null;
        }

        Debug.Log("Flower instantiated successfully.");

        // Add offset to the Y-axis to adjust the placement
        float yOffset = 5.0f; // Adjust this value as needed
        newFlower.transform.position = new Vector3(position.x, position.y + yOffset, position.z);

        // Initialize FlowerDataManager
        FlowerDataManager flowerDataManager = newFlower.GetComponent<FlowerDataManager>();
        if (flowerDataManager == null)
        {
            Debug.LogError("FlowerDataManager component is missing on the prefab!");
            Destroy(newFlower); // Cleanup to prevent orphaned GameObjects
            return null;
        }

        // Initialize FlowerData
        if (flowerDataManager.flowerData == null)
        {
            flowerDataManager.flowerData = new FlowerData
            {
                position = position,
                flowerType = flowerConfig.flowerType
            };
        }
        else
        {
            Debug.LogWarning("FlowerData was already initialized. Reusing existing data.");
        }

        // Perform additional initialization
        flowerDataManager.Initialize();

        // Create a Flower object and add it to the inventory as a planted flower
        Flower flower = new Flower(flowerConfig);
        flower.isPlanted = true; // Mark the flower as planted
        inventory.AddPlantedFlower(flower);

        Debug.Log("Flower added to inventory as a planted flower.");
        return newFlower;
    }


    public List<FlowerConfig> allFlowerConfigs = new List<FlowerConfig>();
    //Finds Flower Config Types
    public FlowerConfig GetFlowerConfigByType(string type)
    {
        foreach (var config in allFlowerConfigs)
        {
            if (config.flowerType.Equals(type, System.StringComparison.OrdinalIgnoreCase))
            {
                return config;
            }
        }
        Debug.LogWarning($"FlowerConfig not found for type: {type}");
        return null;
    }

    // Remove the flower from the scene
    public void RemoveFlower(GameObject flower, Inventory inventory)
    {
        if (flower == null)
        {
            Debug.LogWarning("Attempted to remove a null flower.");
            return;
        }

        // Find the corresponding Flower object in the inventory
        FlowerConfig flowerConfig = flower.GetComponent<FlowerConfig>();
        Flower flowerToRemove = inventory.plantedFlowers.Find(flower => flower.flowerConfig == flowerConfig);

        if (flowerToRemove != null)
        {
            // Remove from inventory
            inventory.RemoveFlower(flowerToRemove);
            Debug.Log($"Removed flower of type {flowerToRemove.flowerConfig.flowerType} from the inventory.");
        }
        else
        {
            Debug.LogWarning("The flower to remove was not found in the inventory.");
        }

        // Destroy the flower GameObject
        Debug.Log($"Destroying flower GameObject: {flower.name}");
        Destroy(flower);
    }

}
