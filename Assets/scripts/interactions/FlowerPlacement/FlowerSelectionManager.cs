using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Links configs with buttons. Will trigger on click
public class FlowerSelectionManager : MonoBehaviour
{
    public static FlowerSelectionManager Instance {get; private set;}
    public HighlightHandler highlightHandler;
    private bool isPlacementModeActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called when a UI button for a specific flower is clicked
    public void OnFlowerButtonPressed(FlowerConfig flowerConfig)
    {
        
        Debug.Log($"Flower selected of type {flowerConfig.flowerType}");

        //IMPLEMENT THIS: check if the inventory has any seeds of that type
        //if so
        // Pass the selected flower configuration to FlowerPlacementController
        isPlacementModeActive = true;
        highlightHandler.HighlightValidPoints();
        FlowerPlacementController.Instance.SetSelectedFlower(flowerConfig);
    }

    public bool IsPlacementModeActive()
    {
        return isPlacementModeActive;
    }

    public void ExitPlacementMode()
    {
        isPlacementModeActive = false;
        highlightHandler.ClearHighlights();
    }
    
}
