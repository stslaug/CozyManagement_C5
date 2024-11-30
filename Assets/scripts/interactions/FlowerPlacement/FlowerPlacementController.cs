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
    public FlowerManager flowerManager; // Reference to FlowerManager
    public PlacementManager placementManager; // Reference to PlacementManager
    private FlowerConfig selectedFlowerConfig; // Currently selected flower type
    public InventoryManagement inventoryManager;

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

      
    }

    // Handle flower placement logic
    private void HandleFlowerPlacement()
    {
        if (selectedFlowerConfig == null)
        {
            Debug.LogWarning("No flower selected for placement!");
            return;
        }

        // Convert mouse position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;


        // Find the placement point at the clicked position
        PlacementPoint placementPoint = placementManager.GetPointAtPosition(worldPosition);

        if (placementPoint != null && placementPoint.IsAvailable())
        {
            FlowerConfig tempConfig = selectedFlowerConfig;
     
            placementManager.ClearHighlights();

            GameObject flower = flowerManager.SpawnFlower(placementPoint.transform.position, selectedFlowerConfig);
            selectedFlowerConfig = null;
            if (flower != null)
            {
                placementPoint.OccupyPoint();
            }
            else
            {
                Debug.LogWarning("Failed to spawn flower! Check FlowerConfig prefab.");
                return;
            }

            inventoryManager.SubFire_Seed(1);



        }
        else
        {
            Debug.LogWarning("Invalid placement point or point is occupied.");
            return;
        }
    }


    public void placeAllFlowers()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("rooftop_garden"))
        {
            if (GameManager.Instance != null)
            {
                GameManager gameManager = GameManager.Instance;
                if (gameManager.getSaveData().allFlowers.Count != 0)
                {

                    //place all flowers in from allFlowers in the scene
                }
            }
        } 
     }



}
