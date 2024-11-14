using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryAndPlacingManager : MonoBehaviour
{
    // Reference to the UI Button
    public Button button;

    // Reference to the prefab to instantiate
    public GameObject prefab;

    // LayerMask to specify which layers to consider when checking for existing objects
    public LayerMask placementLayerMask;

    // Flag to determine if we are waiting for the next mouse click
    private bool waitingForNextClick = false;

    // Radius to check if position is occupied
    public float checkRadius = 0.5f;

    // Reference to the EventSystem (optional caching for performance)
    private EventSystem eventSystem;

    void Start()
    {
        // Find the button named "FirePlanter" in the scene
        button = GameObject.Find("FirePlanter")?.GetComponent<Button>();

        if (button != null)
        {
            // Assign the OnButtonClick method to the button's onClick event
            button.onClick.AddListener(OnButtonClick);
            Debug.Log("Button 'FirePlanter' found and listener added.");
        }
        else
        {
            Debug.LogError("Button named 'FirePlanter' not found in the scene.");
        }

        if (prefab == null)
        {
            Debug.LogError("Prefab reference is not set in the inspector.");
        }

        // Cache the EventSystem
        eventSystem = EventSystem.current;
        if (eventSystem == null)
        {
            Debug.LogError("No EventSystem found in the scene. Please add one (GameObject > UI > Event System).");
        }
    }

    void Update()
    {
        if (waitingForNextClick)
        {
            // Check for left mouse button down
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the click is not over a UI element
                if (!IsPointerOverUIObject())
                {
                    // Perform the desired action: Instantiate the prefab at mouse position
                    Vector3 spawnPosition = GetMouseWorldPosition();

                    if (spawnPosition != Vector3.zero)
                    {
                        // Check if the position is free (not occupied by existing objects)
                        if (!IsPositionOccupied(spawnPosition))
                        {
                            spawnPosition.z -= 0.05f;
                            Instantiate(prefab, spawnPosition, Quaternion.identity);
                            Debug.Log("Prefab instantiated at: " + spawnPosition);

                            // Optionally, add to inventory or perform other actions
                            addFireFlower();
                        }
                        else
                        {
                            Debug.LogWarning("Cannot place prefab here. Position is already occupied.");
                        }

                        // Reset the flag
                        waitingForNextClick = false;
                    }
                    else
                    {
                        Debug.LogWarning("Unable to determine mouse position in the world.");
                    }
                }
                else
                {
                    Debug.Log("Click was on a UI element. Ignoring.");
                }
            }
        }
    }

    // Method called when the button is clicked
    void OnButtonClick()
    {
        if (prefab == null)
        {
            Debug.LogError("Cannot wait for click because prefab is not assigned.");
            return;
        }

        Debug.Log("Button was clicked. Waiting for the next mouse click to place the prefab.");
        waitingForNextClick = true;
    }

    // Check if the mouse is over a UI element to prevent unintended clicks
    private bool IsPointerOverUIObject()
    {
        if (eventSystem == null)
        {
            // Attempt to find the EventSystem if not cached
            eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                Debug.LogError("No EventSystem found in the scene.");
                return false;
            }
        }

        // Create a PointerEventData object
        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        // Raycast against UI elements
        var results = new System.Collections.Generic.List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        bool isOverUI = results.Count > 0;
        if (isOverUI)
        {
            Debug.Log("Pointer is over a UI element.");
        }
        return isOverUI;
    }

    // Convert mouse position to world position in 2D
    private Vector3 GetMouseWorldPosition()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera not found. Please tag your camera as 'MainCamera'.");
            return Vector3.zero;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(cam.transform.position.z); // Ensure correct z-distance

        Vector3 worldPosition = cam.ScreenToWorldPoint(mousePos);
        worldPosition.z = 0f; // Ensure z=0 for 2D

        Debug.Log("Mouse World Position: " + worldPosition);
        return worldPosition;
    }

    // Check if the spawn position is already occupied using 2D physics
    private bool IsPositionOccupied(Vector3 position)
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(position, checkRadius, placementLayerMask);
        if (hitCollider != null)
        {
            Debug.Log("Position " + position + " is already occupied by " + hitCollider.name + ".");
            return true;
        }
        return false;
    }

    // Visualize the placement radius in the Scene view for 2D
    void OnDrawGizmosSelected()
    {
        if (waitingForNextClick)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            if (spawnPosition != Vector3.zero)
            {
                Gizmos.color = IsPositionOccupied(spawnPosition) ? Color.red : Color.green;
                Gizmos.DrawWireSphere(spawnPosition, checkRadius);
            }
        }
    }

    // Add a Fire Flower to the inventory and update UI
    public void addFireFlower()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        GameManager.Instance.saveData.inventoryData.fireFlowerCount += 1;
        Debug.Log("FireFlower count incremented to: " + GameManager.Instance.saveData.inventoryData.fireFlowerCount);

        TextMeshProUGUI textTemp = GameObject.Find("FirePlanter_text")?.GetComponent<TextMeshProUGUI>();
        if (textTemp != null)
        {
            textTemp.text = GameManager.Instance.saveData.inventoryData.fireFlowerCount.ToString();
            Debug.Log("FirePlanter_text updated to: " + textTemp.text);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component 'FirePlanter_text' not found.");
        }
    }
}
