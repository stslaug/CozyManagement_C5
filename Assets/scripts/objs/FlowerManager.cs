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
    public GameObject SpawnFlower(Vector3 position, FlowerConfig flowerConfig)
    {
        if (flowerConfig.prefab != null)
        {
            Debug.Log($"Spawning flower: {flowerConfig.flowerType} at {position}");

            // Instantiate the flower prefab
            GameObject newFlower = Instantiate(flowerConfig.prefab, position, Quaternion.identity);

            if (newFlower != null)
            {
                Debug.Log($"Flower instantiated successfully");

                // Add offset to the Y-axis to adjust the placement
                float yOffset = 5.0f; // Adjust this value as needed
                newFlower.transform.position = new Vector3(position.x, position.y + yOffset, position.z);

                // Initialize FlowerDataManager
                FlowerDataManager flowerDataManager = newFlower.GetComponent<FlowerDataManager>();
                if (flowerDataManager != null)
                {
                    if (flowerDataManager.flowerData == null)
                    {
                        flowerDataManager.flowerData = new FlowerData();
                        flowerDataManager.flowerData.position = position;
                        flowerDataManager.flowerData.flowerType = flowerConfig.flowerType;
                        flowerDataManager.Initialize();
                        // Add flower to GameManager tracking
                        gameManager.AddFlower(newFlower);
                        Debug.Log("Flower added to GameManager.");
                        return newFlower;
                    }
                    else
                    {
                        Debug.Log("FlowerDataManager script missing on prefab!  Creating one...");
                        return null;

                    }

                }
                else
                {
                    Debug.LogError("Flower instantiation failed!");
                    return null;
                }

            }
            else
            {
                Debug.LogError("FlowerConfig prefab is null!");
                return null;
            }
        }
        Debug.LogWarning("Prefab = null | Flower Manager");
        return null;
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
    public void RemoveFlower(GameObject flower)
    {
        if (flower != null)
        {
         
                    gameManager.RemoveFlower(flower);
                    Debug.Log($"Removing flower from the scene."); // Debug: removing flower 
            Destroy(flower);
        }
        else
        {
            Debug.LogWarning("Attempted to remove null flower."); // Debug: trying to remove null flower
        }
    }
}
