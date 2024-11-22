using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataModels;

//manages flower placing logic
public class itemPlacement : MonoBehaviour
{
    public FlowerData selectedFlowerData;
    public GameObject fireFlowerPrefab;
    public GameObject waterFlowerPrefab;
    public GameObject windFlowerPrefab;

    private GameManager gameManager;
    private bool isPlacingItem = false;
    private GameObject itemToPlace;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the GameManager instance
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            Debug.LogError("GameManager instance not found! Ensure GameManager is initialized.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleItemPlacement();
    }

    public void PrepareToPlaceItem(string itemType)
    {
        if (gameManager == null) return;

        if (itemType == "FireFlower")
        {
            if (gameManager.saveData.inventoryData.fire_seed > 0)
            {
                itemToPlace = fireFlowerPrefab;
                isPlacingItem = true;
            }
            else
            {
                Debug.Log("Out of Fire Flowers.");
            }
        }
        else if (itemType == "WaterFlower")
        {
            if (gameManager.saveData.inventoryData.water_seed > 0)
            {
                itemToPlace = waterFlowerPrefab;
                isPlacingItem = true;
            }
            else
            {
                Debug.Log("Out of Water Flowers.");
            }
        }
        else if (itemType == "WindFlower")
        {
            if (gameManager.saveData.inventoryData.wind_seed > 0)
            {
                itemToPlace = windFlowerPrefab;
                isPlacingItem = true;
            }
            else
            {
                Debug.Log("Out of Wind Flowers.");
            }
        }
        else
        {
            Debug.LogWarning("Unknown item type.");
        }
    }

private void HandleItemPlacement()
    {
        if (isPlacingItem && Input.GetMouseButtonDown(0))
        {
            if (SceneManager.GetActiveScene().name == "temp_rooftop" && itemToPlace != null)
            {
                Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(
                    Input.mousePosition.x, 
                    Input.mousePosition.y, 
                    -Camera.main.transform.position.z));
                spawnPosition.z = 0; // Ensure placement on the correct plane

                RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
                if (hit.collider == null)
                {
                    Instantiate(itemToPlace, spawnPosition, Quaternion.identity);
                    UpdateInventoryCount();
                    Debug.Log($"Item {itemToPlace.name} placed at: {spawnPosition}");
                    isPlacingItem = false;
                }
                else
                {
                    Debug.LogWarning("Cannot place item on another object. Cancelling placement.");
                    isPlacingItem = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPlacingItem = false;
            Debug.Log("Item placement cancelled.");
        }
    }

    private void UpdateInventoryCount()
    {
        if (itemToPlace == fireFlowerPrefab)
        {
            gameManager.saveData.inventoryData.fire_seed--;
        }
        else if (itemToPlace == waterFlowerPrefab)
        {
            gameManager.saveData.inventoryData.water_seed--;
        }
        else if (itemToPlace == windFlowerPrefab)
        {
            gameManager.saveData.inventoryData.wind_seed--;
        }
    }
}
