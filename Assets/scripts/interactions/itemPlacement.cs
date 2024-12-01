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

    private FlowerManager flowerManager;
    private Inventory inventory;
    private bool isPlacingItem = false;
    private GameObject itemToPlace;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the GameManager instance
        if (inventory == null)
        {
            inventory = GameObject.Find("inventoryData").GetComponent<Inventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleItemPlacement();
    }

    public void PrepareToPlaceItem(string itemType)
    {
        if (GameManager.Instance == null) return;

        if (itemType == "FireFlower")
        {
            if (inventory.GetUnplantedFlowerCount(flowerManager.GetFlowerConfigByType("Fire")) > 0)

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
            if (inventory.GetUnplantedFlowerCount(flowerManager.GetFlowerConfigByType("water")) > 0)
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
            if (inventory.GetUnplantedFlowerCount(flowerManager.GetFlowerConfigByType("wind")) > 0)
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
}
