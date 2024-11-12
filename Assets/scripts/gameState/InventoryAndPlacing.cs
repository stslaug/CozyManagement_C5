using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class InventoryAndPlacing : MonoBehaviour
{
    public static GameManager gameManager;
    public static InventoryAndPlacing Instance;

    public TextMeshProUGUI FireFlowerText;

    public GameObject FireFlowerPrefab;

    private static bool placingFireFlower = false;
    private int tempFireFlowerCount = 0;

    void Awake()
    {
        tempFireFlowerCount = -1;
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize GameManager reference
        if (GameManager.Instance != null)
        {
            gameManager = GameManager.Instance;
        }
        else
        {
            Debug.LogError("GameManager instance not found! Ensure GameManager is initialized as a singleton.");
        }
        updateFireTextDisplay();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlacing_FireFlower();
        handleFlowerInteract();
        updateFireTextDisplay();
    }

    public void updateFireTextDisplay()
    {

        if(tempFireFlowerCount != gameManager.inventoryData.fireFlowerCount)
        {
            GameObject fftCheck = GameObject.Find("FirePlanter_text");
            if (fftCheck != null)
            {

                FireFlowerText = fftCheck.GetComponent<TextMeshProUGUI>();
                FireFlowerText.text = GameManager.Instance.inventoryData.fireFlowerCount.ToString();
                tempFireFlowerCount = GameManager.Instance.inventoryData.fireFlowerCount;
            }
            else
            {
                Debug.Log("Can't Find FirePlanter_text");
            }
        }
       
            


        return;
       
    }

    public void FireFlowerPlanter()
    {
        Debug.Log("Attempting to play flower...");
        if (gameManager != null)
        {
            if (GameManager.Instance != null)
            {
                gameManager = GameManager.Instance;
            }
            if (gameManager != null)
            {
                if (gameManager.inventoryData.fireFlowerCount > 0)
                {
                    placingFireFlower = true;

                    return;
                }
                else
                {
                    Debug.Log("Out of Fire Flowers.");
                    return;
                }
            }

        }
        else
        {
            Debug.LogError("GameManager reference is null.");
            return;
        }


    }


    private void handleFlowerInteract()
    {
        // Only proceed if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Only proceed if the current scene is "temp_rooftop"
            if (SceneManager.GetActiveScene().name == "temp_rooftop")
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
            }
        }
    }


    private void HandlePlacing_FireFlower()
    {
        if (placingFireFlower == true)
        {
            Debug.Log("Handling FireFlower Placement");


            if (Input.GetMouseButtonDown(0))
            {

                // Only proceed if the current scene is "temp_rooftop"
                if (SceneManager.GetActiveScene().name == "temp_rooftop")
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
                        placingFireFlower = false;
                        Debug.LogWarning("Can't Place Flower On another object... Canceling Placement.");

                        return;
                    }
                    placingFireFlower = false;
                    


                    // Instantiate the flower prefab at the click position if no flower was clicked
                    GameObject newFlower = Instantiate(FireFlowerPrefab, spawnPosition, Quaternion.identity);
                    Flower flowerComponent = newFlower.GetComponent<Flower>();

                    // Add the new flower's data to the GameManager's flowerData list
                    if (flowerComponent != null && gameManager != null)
                    {
                        gameManager.flowerData.Add(flowerComponent.flowerData);
                    }

                    Debug.Log("Flower spawned at: " + spawnPosition);
                    gameManager.inventoryData.fireFlowerCount -= 1;
                    

                

                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                placingFireFlower = false;

                return;
            }

        }

    }



    public void addFireFlowerSeed(int num)
    {
        gameManager.inventoryData.fireFlowerCount += num;
    }






}