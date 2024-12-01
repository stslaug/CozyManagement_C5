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
    public static FlowerPlacementController Instance {get; private set;}
    public FlowerManager flowerManager; // Reference to FlowerManager
    public PlacementManager placementManager; // Reference to PlacementManager
    private FlowerConfig selectedFlowerConfig; // Currently selected flower type
    public InventoryManagement inventoryManager;

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

    // Handle flower placement logic
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

    /*
    public void placeAllFlowers()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("rooftop_garden"))
        {
            if (GameManager.Instance != null)
            {

                if (GameManager.Instance.saveData.allFlowers.Count != 0)
                {

                    //place all flowers in from allFlowers in the scene
                }
            }
        } 
     }
    */

}
