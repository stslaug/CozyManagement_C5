using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnClickAndHover : MonoBehaviour
{
    public Texture2D hoverCursor;          // Assign your cursor texture in the Inspector
    public GameObject flowerPrefab;        // Assign your Flower prefab in the Inspector
    public GameManager gameManager;
    public PauseMenuController pauseMenu;

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        }

        // Find the pause menu in the current scene
        FindPauseMenu();

        // Set the default cursor
        Cursor.SetCursor(null, hotSpot, cursorMode);

        // Subscribe to the sceneLoaded event to find the pause menu when a new scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the pause menu in the new scene
        FindPauseMenu();
    }

    private void FindPauseMenu()
    {
        GameObject pauseMenuObject = GameObject.Find("PauseMenu");
        if (pauseMenuObject != null)
        {
            pauseMenu = pauseMenuObject.GetComponent<PauseMenuController>();
        }
        else
        {
            pauseMenu = null;
            Debug.Log("PauseMenu not found in the current scene.");
        }
    }

    private void Update()
    {
        // Only proceed if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Only proceed if the current scene is "temp_rooftop"
            if (SceneManager.GetActiveScene().name == "temp_rooftop")
            {
                // Only proceed if the pause menu is not active
                if (pauseMenu == null || !pauseMenu.getPause())
                {
                    
                    
                        // Get the mouse position in world coordinates
                        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(
                            Input.mousePosition.x,
                            Input.mousePosition.y,
                            -Camera.main.transform.position.z)); // Ensure correct distance

                        spawnPosition.z = 0; // Set Z to 0 for a 2D game

                        // Cast a ray to check if clicking on an existing flower
                        RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
                        if (hit.collider != null)
                        {
                            Flower flower = hit.collider.GetComponent<Flower>();
                            if (flower != null)
                            {
                                flower.Interact(); // Grow the flower
                                return;
                            }
                        }

                        // Instantiate the flower prefab at the click position if no flower was clicked
                        GameObject newFlower = Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
                        Flower flowerComponent = newFlower.GetComponent<Flower>();

                        // Add the new flower's data to the GameManager's flowerData list
                        if (flowerComponent != null && gameManager != null)
                        {
                            gameManager.flowerData.Add(flowerComponent.flowerData);
                        }

                        Debug.Log("Flower spawned at: " + spawnPosition);
                    
                }
            }
        }
    }
}
