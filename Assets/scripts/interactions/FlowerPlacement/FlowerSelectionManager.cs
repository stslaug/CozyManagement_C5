using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Links configs with buttons. Will trigger on click
public class FlowerSelectionManager : MonoBehaviour
{
    public FlowerPlacementController placementController; // Reference to FlowerPlacementController

    // Called when a UI button for a specific flower is clicked
    public void SelectFlower(FlowerConfig flowerConfig)
    {
        // Pass the selected flower configuration to FlowerPlacementController
        placementController.SetSelectedFlower(flowerConfig);
    }
}
