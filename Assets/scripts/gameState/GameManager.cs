using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance;

    // References to other trackers
    public CurrencyTracker currencyTracker;
    public DaysTracker daysTracker;

    // UI Components
    [SerializeField] private GameObject pauseMenu;          // Assign via Inspector
    [SerializeField] private GameObject seasonPanel;        // Assign via Inspector
    [SerializeField] private Image seasonPanelImage;         // Assign via Inspector

    // Save System Variables
    private string saveDirectory;
    private readonly string[] sceneNames = { "temp_rooftop", "temp_shop" };

    // Game Data
    public PlayerData playerData;
    public SaveData saveData;
    public List<NPCData> npcData = new List<NPCData>();
    public List<FlowerData> flowerData = new List<FlowerData>();
    public InventoryData inventoryData;
    public GameObject flowerPrefab;                         // Assign via Inspector

    void Awake()
    {
        // Singleton Pattern Enforcement
        if (Instance == null)
        {
            Instance = this;
            saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");

            // Create Save Directory if it doesn't exist
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            // Ensure this GameObject persists across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadGame()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        // Reset current data
        saveData = null;
        playerData = null;
        npcData.Clear();
        flowerData.Clear();
        // Initialize inventoryData if needed

        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                saveData = JsonUtility.FromJson<SaveData>(json);

                if (saveData == null)
                {
                    Debug.LogError("Failed to deserialize SaveData from JSON.");
                    InitializeDefaultData();
                    return;
                }

                // Assign loaded data
                playerData = saveData.playerData;
                npcData = saveData.npcData;
                flowerData = saveData.flowerData;
                inventoryData = saveData.inventoryData;

                if (playerData == null)
                {
                    Debug.LogError("playerData is null in loaded SaveData.");
                    InitializeDefaultData();
                    return;
                }

                Debug.Log("Game loaded successfully. Switching scenes...");

                // Load the last scene
                if (!string.IsNullOrEmpty(playerData.lastScene))
                {
                    Debug.Log($"Attempting to load scene: {playerData.lastScene}");

                    if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
                    {
                        SceneManager.sceneLoaded += OnSceneLoaded;
                        SceneManager.LoadScene(playerData.lastScene);
                    }
                    else
                    {
                        Debug.LogError($"Scene '{playerData.lastScene}' cannot be loaded.");
                        InitializeDefaultData();
                    }
                }
                else
                {
                    Debug.LogError("playerData.lastScene is null or empty.");
                    InitializeDefaultData();
                }
            }
            else
            {
                Debug.LogWarning("Save file not found. Initializing default player data.");
                InitializeDefaultData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading save file from {filePath}: {e.Message}\n{e.StackTrace}");
            InitializeDefaultData();
        }
    }

    private void InitializeDefaultData()
    {
        // Initialize default data
        saveData = new SaveData();
        playerData = new PlayerData
        {
            creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            lastScene = "temp_shop",
           // currentDay = 1
        };
        npcData = new List<NPCData>();
        flowerData = new List<FlowerData>();
        inventoryData = new InventoryData(); // Ensure InventoryData has a default constructor

        saveData.playerData = playerData;
        saveData.npcData = npcData;
        saveData.flowerData = flowerData;
        saveData.inventoryData = inventoryData;

        Debug.Log($"Loading default scene: {playerData.lastScene}");

        if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(playerData.lastScene);
        }
        else
        {
            Debug.LogError($"Default scene '{playerData.lastScene}' cannot be loaded.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Instantiate flowers in the loaded scene
        InstantiateFlowersInScene(scene.name);

        // Update UI Trackers
        UpdateTrackers();

        // Handle special events like Yeti appearance
        HandleSpecialEvents(scene.name);

        // Unsubscribe to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InstantiateFlowersInScene(string sceneName)
    {
        if (flowerData != null)
        {
            foreach (FlowerData data in flowerData)
            {
                if (data.scene_name == sceneName)
                {
                    if (flowerPrefab != null)
                    {
                        GameObject flowerObject = Instantiate(flowerPrefab, data.position, Quaternion.identity);
                        Flower flower = flowerObject.GetComponent<Flower>();

                        if (flower != null)
                        {
                            flower.flowerData = data;
                            flower.ApplyFlowerDataChanges();
                        }
                        else
                        {
                            Debug.LogError("Flower component not found on flowerPrefab.");
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
    }

    private void UpdateTrackers()
    {
        // Update Currency Tracker
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
            Debug.LogError("CurrencyTracker component not found.");
        }

        // Update Days Tracker
        if (daysTracker == null)
        {
            daysTracker = GameObject.Find("daysTracker")?.GetComponent<DaysTracker>();
        }

        if (daysTracker != null)
        {
            daysTracker.UpdateDayDisplay();
        }
        else
        {
            Debug.LogError("DaysTracker component not found.");
        }
    }

    private void HandleSpecialEvents(string sceneName)
    {
        if (sceneName == "temp_shop")
        {
            if (Instance.playerData.currentDay == 3)
            {
                Debug.Log("Activating Yeti");
                ActivateYetiRequest();
            }
            if (Instance.playerData.currentDay == 0 || Instance.playerData.currentDay == 5)
            {
                Debug.Log("Deactivating Yeti");
                DeactivateYetiRequest();
            }
        }

        // Add more special event handlers as needed
    }

    private void ActivateYetiRequest()
    {
        GameObject yetiRequest = GameObject.Find("Yeti_Request");

        if (yetiRequest == null)
        {
            GameObject playerUI = GameObject.Find("Player_UI");

            if (playerUI != null)
            {
                Transform yetiTransform = playerUI.transform.Find("Yeti_Request");

                if (yetiTransform != null)
                {
                    yetiRequest = yetiTransform.gameObject;
                }
                else
                {
                    Debug.LogError("Yeti_Request GameObject not found under Player_UI.");
                }
            }
            else
            {
                Debug.LogError("Player_UI GameObject not found.");
            }
        }

        if (yetiRequest != null)
        {
            yetiRequest.SetActive(true);
            Debug.Log("Yeti has appeared!");
        }
        else
        {
            Debug.LogError("Yeti GameObject not found.");
        }
    }

    private void DeactivateYetiRequest()
    {
        GameObject yetiRequest = GameObject.Find("Yeti_Request");

        if (yetiRequest == null)
        {
            GameObject playerUI = GameObject.Find("Player_UI");

            if (playerUI != null)
            {
                Transform yetiTransform = playerUI.transform.Find("Yeti_Request");

                if (yetiTransform != null)
                {
                    yetiRequest = yetiTransform.gameObject;
                }
                else
                {
                    Debug.LogError("Yeti_Request GameObject not found under Player_UI.");
                }
            }
            else
            {
                Debug.LogError("Player_UI GameObject not found.");
            }
        }

        if (yetiRequest != null)
        {
            yetiRequest.SetActive(false);
            Debug.Log("Yeti has left!");
        }
        else
        {
            Debug.LogError("Yeti GameObject not found.");
        }
    }

    public void SaveGame()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        // Initialize saveData if null
        if (saveData == null)
            saveData = new SaveData();

        // Assign playerData, npcData, flowerData, and inventoryData to saveData
        saveData.playerData = playerData;
        saveData.npcData = npcData;
        saveData.flowerData = flowerData;
        saveData.inventoryData = inventoryData;

        // Update playerData with current time and scene
        playerData.lastTimePlayed = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        playerData.lastScene = SceneManager.GetActiveScene().name;

        // Remove existing flowers in the current scene from flowerData
        string currentSceneName = SceneManager.GetActiveScene().name;
        flowerData.RemoveAll(data => data.scene_name == currentSceneName);

        // Find all Flower instances in the scene and add their data
        Flower[] flowers = FindObjectsOfType<Flower>();
        foreach (Flower flower in flowers)
        {
            if (flower.flowerData != null)
            {
                FlowerData data = new FlowerData
                {
                    position = flower.transform.position,
                    scene_name = currentSceneName,
                    growthStep = flower.flowerData.growthStep,
                    growthRate = flower.flowerData.growthRate,
                    flowerType = flower.flowerData.flowerType,
                    seasonsAllowed = new List<Season>(flower.flowerData.seasonsAllowed),
                    currentNeeds = new List<Need>(flower.flowerData.currentNeeds)
                };

                flowerData.Add(data);
            }
            else
            {
                Debug.LogWarning($"Flower '{flower.gameObject.name}' has no FlowerData assigned.");
            }
        }

        // Serialize saveData to JSON
        string json = JsonUtility.ToJson(saveData, true); // 'true' for pretty print

        try
        {
            File.WriteAllText(filePath, json);
            Debug.Log($"Save successful. File Path: {filePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving to {filePath}: {e.Message}");
        }
    }

    public void DeleteSave()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log($"Save file {filePath} deleted.");
            }
            catch (Exception e)
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
        if (!SceneManager.GetActiveScene().name.Equals("mainMenu", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("mainMenu");

            // Optionally, clear game data if needed
            // saveData = null;
            // playerData = null;
            // npcData.Clear();
            // flowerData.Clear();
            // inventoryData = null;
        }
        else
        {
            Debug.Log("User has initiated exiting the game");
            Application.Quit();
        }
    }

    public void ChangeScene()
    {
        int currentSceneIndex = Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        int nextSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;

        SaveGame();

        if (!string.IsNullOrEmpty(sceneNames[nextSceneIndex]))
        {
            Debug.Log($"Attempting to load scene: {sceneNames[nextSceneIndex]}");

            if (Application.CanStreamedLevelBeLoaded(sceneNames[nextSceneIndex]))
            {
                if (sceneNames[nextSceneIndex] == "temp_shop")
                {
                    Debug.Log("Ending Day...");
                    Instance.playerData.currentDay += 1;

                    if (daysTracker != null)
                        daysTracker.UpdateDayDisplay();
                }
                else
                {
                    Debug.Log($"Scene '{sceneNames[nextSceneIndex]}' loaded.");
                }

                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(sceneNames[nextSceneIndex]);
            }
            else
            {
                Debug.LogError($"Scene '{sceneNames[nextSceneIndex]}' cannot be loaded.");
            }
        }
        else
        {
            Debug.LogError("Next scene name is null or empty.");
        }
    }

    public void UpdateAllFlowers(Action<FlowerData> updateAction)
    {
        if (updateAction == null)
        {
            Debug.LogError("Update action is null.");
            return;
        }

        Flower[] allFlowers = FindObjectsOfType<Flower>();

        if (allFlowers.Length == 0)
        {
            Debug.LogWarning("No Flower objects found in the scene.");
            return;
        }

        foreach (Flower flower in allFlowers)
        {
            if (flower != null && flower.flowerData != null)
            {
                updateAction(flower.flowerData);
                flower.ApplyFlowerDataChanges();
            }
        }

        Debug.Log($"Updated FlowerData for {allFlowers.Length} flowers.");
    }

    public void ChangeSeasonSpell(Season newSeason, Color panelColor)
    {
        // Update the SeasonManager's current season
        SeasonManager.Instance.ChangeSeason(newSeason);

        // Update all flowers to match seasonal needs
        UpdateAllFlowers(flowerData =>
        {
            // Adjust growth capabilities and needs based on the new season
            switch (newSeason)
            {
                case Season.Spring:
                    flowerData.seasonsAllowed = new List<Season> { Season.Spring };
                    flowerData.currentNeeds = new List<Need> { Need.Water, Need.Sunlight };
                    break;
                case Season.Summer:
                    flowerData.seasonsAllowed = new List<Season> { Season.Summer };
                    flowerData.currentNeeds = new List<Need> { Need.Water, Need.Sunlight };
                    break;
                case Season.Fall:
                    flowerData.seasonsAllowed = new List<Season> { Season.Fall };
                    flowerData.currentNeeds = new List<Need> { Need.Water };
                    break;
                case Season.Winter:
                    flowerData.seasonsAllowed = new List<Season> { Season.Winter };
                    flowerData.currentNeeds = new List<Need>(); // No needs
                    break;
            }
        });

        // Update the Season Panel UI
        if (seasonPanel != null && seasonPanelImage != null)
        {
            seasonPanel.SetActive(true);
            seasonPanelImage.color = panelColor;
            Debug.Log("SeasonPanel color changed.");
        }
        else
        {
            Debug.LogError("SeasonPanel or its Image component is not assigned in the Inspector.");
        }
    }
}
