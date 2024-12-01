using DataModels;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//Determines where on screen flowers will be placed
public class FlowerPlacementController : MonoBehaviour
{
    public static FlowerPlacementController Instance { get; private set; }
    public FlowerManager flowerManager; // Reference to FlowerManager
    public PlacementManager placementManager; // Reference to PlacementManager
    private FlowerConfig selectedFlowerConfig; // Currently selected flower type
    public Inventory inventoryManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }
    // Called by FlowerSelectionManager to set the selected flower type
    public void SetSelectedFlower(FlowerConfig flowerConfig)
    {
        selectedFlowerConfig = flowerConfig;
        Debug.Log($"Set flower type: {selectedFlowerConfig.flowerType}");
    }

    // Called every frame to detect player clicks
    private void Update()
    {
        if (FlowerSelectionManager.Instance.IsPlacementModeActive())
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlacementPoint placementPoint = placementManager.GetPointUnderMouse();
                if (placementPoint != null)
                {
                    Debug.Log("Have a placement point.");
                    PlaceFlower(placementPoint);
                }
                else
                {
                    Debug.Log("Not a valid placement point.");
                }
                selectedFlowerConfig = null;
                FlowerSelectionManager.Instance.ExitPlacementMode();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && selectedFlowerConfig != null)
            {
                Debug.LogWarning("Flower Placement Canceled!");
                selectedFlowerConfig = null;
                FlowerSelectionManager.Instance.ExitPlacementMode();
            }
        }
    }

    private void PlaceFlower(PlacementPoint placementPoint)
    {
        Debug.Log("Placing flower.");
        if (selectedFlowerConfig == null)
        {
            Debug.LogWarning("No flower selected for placement!");
            return;
        }
        Debug.Log($"Placing a {selectedFlowerConfig.flowerType}");
        // Place the flower using FlowerManager
        GameObject flower = flowerManager.SpawnFlower(placementPoint.transform.position, selectedFlowerConfig);
        placementPoint.OccupyPoint();
        //subtract seed of type
        //IMPLEMENT: inventoryManager.SubFire_Seed(1);  //for any type

        if (flower == null)
        {
            Debug.LogWarning("Failed to spawn flower! Check FlowerConfig prefab.");
        }
    }

    // Function to place all planted flowers on the scene
    public void placeAllFlowers()
    {

        // Ensure we're in the correct scene
        if (SceneManager.GetActiveScene().name != "rooftop_garden")
        {
            Debug.LogWarning("Current scene is not 'rooftop_garden'. Flowers will not be placed.");
            return;
        }

        // Check if GameManager and SaveData are available
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager instance is null.");
            return;
        }

        SaveData saveData = GameManager.Instance.getSaveData();
        if (saveData == null)
        {
            Debug.LogWarning("SaveData is null.");
            return;
        }

        // Check if there are flowers to place
        if (inventoryManager.plantedFlowers.Count == 0)
        {
            Debug.Log("No planted flowers to place.");
            return;
        }

        foreach (var flower in inventoryManager.plantedFlowers)
        {
            // Instantiate the prefab at the stored position with an upward offset
            Vector3 spawnPosition = flower.position + Vector3.up * 5; // Adjust the offset as needed

            GameObject flowerObject = Instantiate(flower.flowerConfig.prefab, spawnPosition, Quaternion.identity);

        }

        Debug.Log("All planted flowers have been placed on the scene.");
    }
}
