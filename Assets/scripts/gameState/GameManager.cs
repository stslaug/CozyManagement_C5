using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static CurrencyTracker currencyTracker;
    public GameObject pauseMenu;
    string saveDirectory;
    private string[] sceneNames = { "temp_rooftop", "temp_shop" };
    public PlayerData playerData;
    public SaveData saveData;
    public List<NPCData> npcData;
    public List<FlowerData> flowerData;
    public InventoryData inventoryData;
    public GameObject flowerPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
            Instance = this;

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

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
    }

    // Call this method to load save data into the GameManager
    public void LoadGame()
    {
        // Ensure saveDirectory is initialized
        if (string.IsNullOrEmpty(saveDirectory))
        {
            saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        }

        string filePath = Path.Combine(saveDirectory, "save.json");

        saveData = null;
        playerData = null;
        npcData = null;
        flowerData = null;
        inventoryData = null;

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SaveData loadedSaveData = JsonUtility.FromJson<SaveData>(json);

                if (loadedSaveData == null)
                {
                    Debug.LogError("Failed to deserialize SaveData from JSON.");
                    return;
                }

                saveData = loadedSaveData;
                playerData = loadedSaveData.playerData;
                npcData = loadedSaveData.npcData;
                flowerData = loadedSaveData.flowerData;
                inventoryData = loadedSaveData.inventoryData;

                if (playerData == null)
                {
                    Debug.LogError("playerData is null in loaded SaveData.");
                    return;
                }

                Debug.Log("Game loaded successfully. Switching scenes...");

                // Check if playerData.lastScene is valid
                if (!string.IsNullOrEmpty(playerData.lastScene))
                {
                    Debug.Log("Attempting to load scene: " + playerData.lastScene);

                    // Check if the scene can be loaded
                    if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
                    {
                        // Subscribe to the sceneLoaded event
                        SceneManager.sceneLoaded += OnSceneLoaded;
                        SceneManager.LoadScene(playerData.lastScene);
                    }
                    else
                    {
                        Debug.LogError("Scene '" + playerData.lastScene + "' cannot be loaded. Check if it's added to the build settings and the name is correct.");
                    }
                }
                else
                {
                    Debug.LogError("playerData.lastScene is null or empty. Cannot load scene.");
                }
            }
            else
            {
                Debug.LogWarning("Save file not found. Returning default player data.");

                saveData = new SaveData();
                playerData = new PlayerData();
                npcData = new List<NPCData>();
                flowerData = new List<FlowerData>();

                playerData.creationDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                playerData.lastScene = "temp_rooftop";

                saveData.playerData = playerData;
                saveData.npcData = npcData;
                saveData.flowerData = flowerData;
                saveData.inventoryData = inventoryData;

                Debug.Log("Loading default scene: " + playerData.lastScene);

                if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene(playerData.lastScene);
                }
                else
                {
                    Debug.LogError("Default scene '" + playerData.lastScene + "' cannot be loaded. Check if it's added to the build settings and the name is correct.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading save file from {filePath}: {e.Message}\n{e.StackTrace}");
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Instantiate flowers from saved data
        if (flowerData != null)
        {
            foreach (FlowerData data in flowerData)
            {
                // Ensure we only instantiate flowers for the current scene
                if (data.scene_name == scene.name)
                {
                    if (flowerPrefab != null)
                    {
                        GameObject flowerObject = Instantiate(flowerPrefab, data.position, Quaternion.identity);
                        Flower flower = flowerObject.GetComponent<Flower>();

                        if (flower != null)
                        {
                            flower.growthStep = data.growthStep;
                            flower.flowerData.flowerType = data.flowerType;
                            flower.UpdateAppearance();
                        }
                    }
                    else
                    {
                        Debug.LogError("Flower prefab is not assigned in the GameManager.");
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No flower data found to instantiate.");
        }

        // Update currency tracker
        if (currencyTracker == null)
        {
            currencyTracker = GameObject.Find("currencyTracker")?.GetComponent<CurrencyTracker>();
        }

        if (currencyTracker != null)
        {
            currencyTracker.UpdateGoldDisplay();
        }
        else
        {
            Debug.LogError("CurrencyTracker component not found. UpdateGoldDisplay could not be called.");
        }

        // Handle pauseMenu
        if (pauseMenu == null)
        {
            pauseMenu = GameObject.Find("PauseMenu");

            if (pauseMenu != null)
            {
                var pauseMenuController = pauseMenu.GetComponent<PauseMenuController>();

                if (pauseMenuController != null && !pauseMenuController.getPause())
                {
                    pauseMenuController.TogglePauseMenu();
                }
            }
            else
            {
                Debug.LogWarning("PauseMenu GameObject not found in the scene.");
            }
        }

        // Unsubscribe from the event to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called when the player decides to save the game
    public void SaveGame()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        if (saveData == null)
            saveData = new SaveData();

        if (saveData.playerData == null)
            saveData.playerData = new PlayerData();

        if (saveData.npcData == null)
            saveData.npcData = new List<NPCData>();

        if (saveData.flowerData == null)
            saveData.flowerData = new List<FlowerData>();

        if (flowerData == null)
            flowerData = new List<FlowerData>();
        
        if (inventoryData == null)
            inventoryData = new InventoryData();

        playerData.lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        playerData.lastScene = SceneManager.GetActiveScene().name;

        // Remove existing flower data for the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        flowerData.RemoveAll(data => data.scene_name == currentSceneName);

        // Collect flower data from the current scene
        Flower[] flowers = FindObjectsOfType<Flower>();  // Find all flowers in the current scene
        foreach (Flower flower in flowers)
        {
            FlowerData data = new FlowerData
            {
                position = flower.transform.position,
                scene_name = currentSceneName,
                growthStep = flower.growthStep,
                growthRate = flower.growthStep / (float)flower.maxGrowthStage,
                flowerType = flower.flowerData.flowerType
            };

            flowerData.Add(data);  // Add to the save list
        }

        saveData.playerData = playerData;
        saveData.npcData = npcData;
        saveData.flowerData = flowerData;
        saveData.inventoryData = inventoryData;

        // Convert the SaveData object to a JSON string
        string json = JsonUtility.ToJson(saveData);

        try
        {
            // Write the JSON to the file
            File.WriteAllText(filePath, json);
            Debug.Log($"Save successful. File Path: {filePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving to {filePath}: {e.Message}");
        }
    }

    public void DeleteSave()
    {
        string filePath = Path.Combine(saveDirectory, $"save.json");

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Save file {filePath} deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error deleting save file {filePath}: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning($"No save file found at {filePath} to delete.");
        }
    }

    public void ExitFunction()
    {
        if (SceneManager.GetSceneByName("mainMenu") != SceneManager.GetActiveScene())
        {
            // If we are not in the main menu, load the main menu scene
            Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("mainMenu");

            saveData = null;
            playerData = null;
            npcData = null;
            flowerData = null;
            inventoryData = null;
        }
        else
        {
            // If we are already in the main menu, quit the game
            Debug.Log("User has initiated exiting the game");
            Application.Quit();
        }
    }

    public void ChangeScene()
    {
        int currentSceneIndex = System.Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        int nextSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;

        Instance.SaveGame();

        if (!string.IsNullOrEmpty(sceneNames[nextSceneIndex]))
        {
            Debug.Log("Attempting to load scene: " + sceneNames[nextSceneIndex]);

            // Check if the scene can be loaded
            if (Application.CanStreamedLevelBeLoaded(sceneNames[nextSceneIndex]))
            {
                // Subscribe to the sceneLoaded event
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(sceneNames[nextSceneIndex]);
            }
            else
            {
                Debug.LogError("Scene '" + sceneNames[nextSceneIndex] + "' cannot be loaded. Check if it's added to the build settings and the name is correct.");
            }
        }
        else
        {
            Debug.LogError("sceneNames[nextSceneIndex] is null or empty. Cannot load scene.");
        }
    }
}
