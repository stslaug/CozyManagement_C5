using DataModels;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerPlacementController : MonoBehaviour
{
    public FlowerManager flowerManager; // Reference to FlowerManager
    public PlacementManager placementManager; // Reference to PlacementManager
    private FlowerConfig selectedFlowerConfig; // Currently selected flower type
    public Inventory inventoryManager;

    // Flag to prevent multiple placements
    private bool flowersPlaced = false;

    // Called by FlowerSelectionManager to set the selected flower type
    public void SetSelectedFlower(FlowerConfig flowerConfig)
    {
        selectedFlowerConfig = flowerConfig;
        placementManager.HighlightValidPoints(); // Highlight valid points for placement
    }

    // Called every frame to detect player clicks
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedFlowerConfig != null)
        {
            HandleFlowerPlacement();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && selectedFlowerConfig != null)
        {
            Debug.LogWarning("Flower Placement Canceled!");
            selectedFlowerConfig = null;
            placementManager.ClearHighlights();
        }
        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            placeAllFlowers();
        }
    }

    // Handle flower placement logic
    private void HandleFlowerPlacement()
    {
        if (selectedFlowerConfig == null)
        {
            Debug.LogWarning("No flower selected for placement!");
            return;
        }

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;

        PlacementPoint placementPoint = placementManager.GetPointAtPosition(worldPosition);
        if (placementPoint != null && placementPoint.IsAvailable())
        {
            // Get a Flower instance from the inventory
            Flower flowerToPlant = inventoryManager.GetUnplantedFlowerOfType(selectedFlowerConfig);

            if (flowerToPlant == null)
            {
                Debug.LogWarning("No unplanted flower of selected type in inventory!");
                return;
            }

            // Remove the flower from unplantedFlowers
            inventoryManager.unplantedFlowers.Remove(flowerToPlant);

            // Update the flower instance
            flowerToPlant.isPlanted = true;
            flowerToPlant.position = placementPoint.transform.position; // With Offset for Pots

            // Add the flower to plantedFlowers
            inventoryManager.plantedFlowers.Add(flowerToPlant);

            // Instantiate the flower GameObject in the scene with an upward offset
            GameObject flowerObject = Instantiate(selectedFlowerConfig.prefab, placementPoint.transform.position + Vector3.up * 5, Quaternion.identity);



            // Clear selection and highlights
            selectedFlowerConfig = null;
            placementManager.ClearHighlights();
            placementPoint.OccupyPoint();
        }
        else
        {
            Debug.LogWarning("Invalid placement point or point is occupied.");
            return;
        }
    }

    // Function to place all planted flowers on the scene
    public void placeAllFlowers()
    {
        // Prevent multiple placements
        if (flowersPlaced)
        {
            Debug.LogWarning("Flowers have already been placed.");
            return;
        }

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

            // Optionally, mark the placement point as occupied
            PlacementPoint placementPoint = placementManager.GetPointAtPosition(flower.position);
            if (placementPoint != null)
            {
                placementPoint.OccupyPoint();
            }
            else
            {
                Debug.LogWarning($"No PlacementPoint found at position {flower.position}.");
            }
        }

        flowersPlaced = true; // Set the flag to prevent re-placing
        Debug.Log("All planted flowers have been placed on the scene.");
    }
}
