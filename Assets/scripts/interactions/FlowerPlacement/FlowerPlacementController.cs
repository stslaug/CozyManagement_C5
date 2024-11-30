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
    public Inventory inventoryManager;

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

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;

        PlacementPoint placementPoint = placementManager.GetPointAtPosition(worldPosition);
        if (placementPoint != null && placementPoint.IsAvailable())
        {
            // Get a Flower instance from the inventory
            Flower flowerToPlant = (inventoryManager.GetUnplantedFlowerOfType(selectedFlowerConfig) );

            if (flowerToPlant == null)
            {
                Debug.LogWarning("No unplanted flower of selected type in inventory!");
                return;
            }

            // Remove the flower from unplantedFlowers
            inventoryManager.unplantedFlowers.Remove(flowerToPlant);

            // Update the flower instance
            flowerToPlant.isPlanted = true;
            flowerToPlant.position = (placementPoint.transform.position); // With Offset for Pots)
          
            // Add the flower to plantedFlowers
            inventoryManager.plantedFlowers.Add(flowerToPlant);

            // Instantiate the flower GameObject in the scene
            GameObject flowerObject = Instantiate(selectedFlowerConfig.prefab, (placementPoint.transform.position + (Vector3.up * 5)), Quaternion.identity);

            // Assign the flower instance to the GameObject (e.g., via a FlowerComponent)
            FlowerDataManager flowerComponent = flowerObject.GetComponent<FlowerDataManager>();
            if (flowerComponent == null)
            {
                Debug.Log("Couldn't set flower instance. FlowerPlacementControler.cs");
            }

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
