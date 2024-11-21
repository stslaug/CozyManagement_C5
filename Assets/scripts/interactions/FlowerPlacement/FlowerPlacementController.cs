using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determines where on screen flowers will be placed
public class FlowerPlacementController : MonoBehaviour
{
    public FlowerManager flowerManager; // Reference to FlowerManager
    public PlacementManager placementManager; // Reference to PlacementManager
    private FlowerConfig selectedFlowerConfig; // Currently selected flower type

    // Called by FlowerSelectionManager to set the selected flower type
    public void SetSelectedFlower(FlowerConfig flowerConfig)
    {
        selectedFlowerConfig = flowerConfig;
        Debug.Log($"Selected flower configuration: {selectedFlowerConfig.flowerType}"); // Debug: show selected flower
        placementManager.HighlightValidPoints(); // Highlight valid points for placement
    }

    // Called every frame to detect player clicks
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedFlowerConfig != null)
        {
            Debug.Log("Mouse clicked for flower placement."); // Debug: log mouse click
            HandleFlowerPlacement();
        }
    }

    // Handle flower placement logic
    private void HandleFlowerPlacement()
    {
        // Convert mouse position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;

        Debug.Log($"Mouse clicked at: {worldPosition}");

        // Find the placement point at the clicked position
        PlacementPoint placementPoint = placementManager.GetPointAtPosition(worldPosition);

        if (placementPoint != null && placementPoint.IsAvailable())
        {
            Debug.Log($"Valid placement point found at: {placementPoint.transform.position}");

            // Place the flower
            flowerManager.SpawnFlower(placementPoint.transform.position, selectedFlowerConfig);
            placementPoint.OccupyPoint();

            Debug.Log("Flower placed, and point marked as occupied.");
        }
        else
        {
            if (placementPoint == null)
                Debug.LogWarning("No placement point found at the clicked position.");
            else
                Debug.LogWarning("Placement point is already occupied.");
        }

        // Clear all highlights regardless of the result
        placementManager.ClearHighlights();
        Debug.Log("All highlights cleared after mouse click.");
    }
}
