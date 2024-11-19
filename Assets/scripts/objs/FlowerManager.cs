using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Instantiates, Removes, and Updates all flower instances 
public class FlowerManager : MonoBehaviour
{
    public GameManager gameManager;

    //add a new first stage flower of specified type at specified position
    public GameObject SpawnFlower(Vector3 position, FlowerConfig flowerConfig)
    {
        if (flowerConfig.prefab != null)
        {

            GameObject newFlower = Instantiate(flowerConfig.prefab, position, Quaternion.identity);

            // Initialize the FlowerDataManager with the FlowerConfig
            FlowerDataManager flowerDataManager = newFlower.GetComponent<FlowerDataManager>();
            if (flowerDataManager != null)
            {
                flowerDataManager.flowerConfig = flowerConfig;  // Assign the shared config data
                flowerDataManager.Initialize();
            }
            gameManager.AddFlower(newFlower);
            return newFlower;
        }
        else
        {
            Debug.LogWarning("FlowerConfig does not have a prefab assigned.");
            return null;
        }
    }

    public void RemoveFlower(GameObject flower)
    {
        if (flower != null)
        {
            gameManager.RemoveFlower(flower);
            Destroy(flower);
        }
        else
        {
            Debug.LogWarning("Attempted to remove null flower.");
        }
    }

    public void UpdateAllFlowers()
    {
        foreach (GameObject flower in gameManager.allFlowers)
        {
            FlowerDataManager flowerDataManager = flower.GetComponent<FlowerDataManager>();
            if (flower != null)
            {
                //update flower growth stage
                flowerDataManager.GrowFlower();
            }
        }

        Debug.Log($"Updated FlowerData for {gameManager.allFlowers} flowers.");
    }
}
