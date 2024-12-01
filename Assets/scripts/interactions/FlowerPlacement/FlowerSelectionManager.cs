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
    public Inventory inventoryManager;
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
        if(inventoryManager == null) inventoryManager = GameObject.Find("inventoryData").GetComponent<Inventory>();
    }

    // Called when a UI button for a specific flower is clicked
    public void OnFlowerButtonPressed(FlowerConfig flowerConfig)
    {
        
        Debug.Log($"Flower selected of type {flowerConfig.flowerType}");

        if(inventoryManager.GetUnplantedFlowerCount(flowerConfig) > 0)
        {
            isPlacementModeActive = true;
            highlightHandler.HighlightValidPoints();
            FlowerPlacementController.Instance.SetSelectedFlower(flowerConfig);
        }
        else
        {
            Debug.Log("Cannot Place Flowers. Out of Seeds");
        }
        
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
