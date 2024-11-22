using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles flower installation
//Instantiates, Removes, and Updates all flower instances 
public class FlowerManager : MonoBehaviour
{
    public GameManager gameManager; // Reference to GameManager

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

                // Add a custom offset to the Y-axis to adjust the placement
                float yOffset = 15.0f; // Adjust this value as needed
                newFlower.transform.position = new Vector3(position.x, position.y + yOffset, position.z);

                // Initialize FlowerDataManager
                FlowerDataManager flowerDataManager = newFlower.GetComponent<FlowerDataManager>();
                if (flowerDataManager != null)
                {
                    flowerDataManager.flowerConfig = flowerConfig;
                    flowerDataManager.Initialize();
                    Debug.Log("FlowerDataManager initialized.");
                }
                else
                {
                    Debug.LogWarning("FlowerDataManager script missing on prefab!");
                }

                // Add flower to GameManager tracking
                gameManager.AddFlower(newFlower);
                Debug.Log("Flower added to GameManager.");
            }
            else
            {
                Debug.LogError("Flower instantiation failed!");
            }

            return newFlower;
        }
        else
        {
            Debug.LogError("FlowerConfig prefab is null!");
            return null;
        }
    }
    

    // Remove the flower from the scene
    public void RemoveFlower(GameObject flower)
    {
        if (flower != null)
        {
            Debug.Log($"Removing flower from the scene."); // Debug: removing flower
            gameManager.RemoveFlower(flower);
            Destroy(flower);
        }
        else
        {
            Debug.LogWarning("Attempted to remove null flower."); // Debug: trying to remove null flower
        }
    }
}
