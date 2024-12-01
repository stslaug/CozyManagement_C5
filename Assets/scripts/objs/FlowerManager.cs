using DataModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles flower installation
//Instantiates, Removes, and Updates all flower instances 
public class FlowerManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to GameManager

    private Dictionary<Flower, GameObject> flowerGameObjectMap = new Dictionary<Flower, GameObject>();

    // Add Flower and GameObject mapping
    public void RegisterFlowerWithGameObject(Flower flower, GameObject flowerGameObject)
    {
        if (!flowerGameObjectMap.ContainsKey(flower))
        {
            flowerGameObjectMap.Add(flower, flowerGameObject);
        }
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

        // Add offset to the Y-axis to adjust the placement
        float yOffset = 5.0f; // Adjust this value as needed\
        newFlower.transform.position = new Vector3(position.x, position.y+yOffset, position.z);

        Debug.Log($"Placed flower: {flowerConfig.flowerType} at {newFlower.transform.position}");

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
        // Create a Flower object with the specified FlowerConfig and add it to the inventory as a planted flower
        Flower flower = new Flower(flowerConfig)
        {
            position = position,
            isPlanted = true // Mark the flower as planted
        };
        inventory.AddPlantedFlower(flower);

        // Register the flower and its GameObject with FlowerManager
        RegisterFlowerWithGameObject(flower, newFlower);


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

    // Remove Flower and GameObject mapping
    public void RemoveFlower(Flower flower, Inventory inventory)
    {
        if (flower != null && flowerGameObjectMap.ContainsKey(flower))
        {
            GameObject flowerGameObject = flowerGameObjectMap[flower];
            
            // Remove the flower logically from the inventory
            inventory.RemoveFlower(flower);

            // Destroy the corresponding GameObject
            Destroy(flowerGameObject);

            // Optionally, remove the mapping
            flowerGameObjectMap.Remove(flower);
        }
    }
}
