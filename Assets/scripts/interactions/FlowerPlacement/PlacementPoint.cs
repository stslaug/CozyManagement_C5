using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures flower placement can only happen in specified regions.
public class PlacementPoint : MonoBehaviour
{
    private GameObject currentFlower; // Tracks the placed flower
    private FlowerManager flowerManager;

    private void Start()
    {
        // Find the FlowerManager in the scene
        flowerManager = FindObjectOfType<FlowerManager>();
    }

    public bool IsOccupied => currentFlower != null;

    public bool TryPlaceFlower(Vector3 position, FlowerConfig flowerConfig)
    {
        if (!IsOccupied && flowerManager != null)
        {
            // Use FlowerManager to spawn the flower
            currentFlower = flowerManager.SpawnFlower(position, flowerConfig);
            return currentFlower != null;
        }
        else
        {
            Debug.Log("Placement point is already occupied or FlowerManager is missing.");
            return false;
        }
    }

    public void RemoveFlower()
    {
        if (currentFlower != null && flowerManager != null)
        {
            flowerManager.RemoveFlower(currentFlower);
            currentFlower = null;
        }
    }
}
