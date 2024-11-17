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
    [SerializeField] private GameObject seasonPanel;
    [SerializeField] private Image seasonPanelImage;         

    // Save System Variables
    private string saveDirectory;
    private readonly string[] sceneNames = { "temp_rooftop", "temp_shop" };

    // Game Data
    public SaveData saveData;
    public GameObject flowerPrefab;
    public PlayerDialogue playerDialogue;


    private void Start()
    {
       
    }
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


                if (saveData.playerData == null)
                {
                    Debug.LogError("playerData is null in loaded SaveData.");
                    InitializeDefaultData();
                    return;
                }

                Debug.Log("Game loaded successfully. Switching scenes...");

                // Load the last scene
                if (!string.IsNullOrEmpty(saveData.playerData.lastScene))
                {
                    Debug.Log($"Attempting to load scene: {saveData.playerData.lastScene}");

                    if (Application.CanStreamedLevelBeLoaded(saveData.playerData.lastScene))
                    {
                        SceneManager.sceneLoaded += OnSceneLoaded;
                        SceneManager.LoadScene(saveData.playerData.lastScene);
                    }
                    else
                    {
                        Debug.LogError($"Scene '{saveData.playerData.lastScene}' cannot be loaded.");
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

    // set default user data to be saved. THis include starting gear and stuff
    private void InitializeDefaultData()
    {
        // Initialize default data
        saveData = new SaveData();
        saveData.playerData = new PlayerData
        {
            goldCount = 500,
            currentDay = 1,
            lastTimePlayed = DateTime.Today.ToString(),
            creationDate = DateTime.Today.ToString(),
            lastScene = "temp_shop",
            spellCast = false,
            rooftopSeason = Season.Spring
        };
        saveData.npcData = new List<NPCData>();
        saveData.flowerData = new List<FlowerData>();
        saveData.inventoryData = new InventoryData(); // Ensure InventoryData has a default constructor

        Debug.Log($"Loading default scene: {saveData.playerData.lastScene}");

        if (Application.CanStreamedLevelBeLoaded(saveData.playerData.lastScene))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(saveData.playerData.lastScene);
        }
        else
        {
            Debug.LogError($"Default scene '{saveData.playerData.lastScene}' cannot be loaded.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Instantiate flowers in the loaded scene
        InstantiateFlowersInScene(scene.name);

        HandleSpecialEvents(scene.name);

        // Unsubscribe to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }





    private void HandleSpecialEvents(string sceneName) { 
    }

private void ActivateYetiRequest() { 
    }


       private void ActivateYetiCompletion()
    {
    }

    
    private void DeactivateYetiCompletion()
    {
    }

    private void DeactivateYetiRequest()
    {
    }

        private void ActivateGrannyRequest()
    {
    }

    private void ActivateGrannyCompletion()
    {
    }

    private void DeactivateGrannyRequest()
    {
    }

     private void DeactivateGrannyCompletion()
    {
    }












    private void InstantiateFlowersInScene(string sceneName)
    {
        if (saveData.flowerData != null)
        {
            foreach (FlowerData data in saveData.flowerData)
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



    public void SaveGame()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        // Initialize saveData if null
        if (saveData == null) InitializeDefaultData();


        // Update playerData with current time and scene
        saveData.playerData.lastTimePlayed = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        saveData.playerData.lastScene = SceneManager.GetActiveScene().name;

        // Remove existing flowers in the current scene from flowerData
        string currentSceneName = SceneManager.GetActiveScene().name;
        saveData.flowerData.RemoveAll(data => data.scene_name == currentSceneName);

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

                saveData.flowerData.Add(data);
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

    // Delete the Save File
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

    //Handles Exiting the Game
    public void ExitFunction()
    {
        if (!SceneManager.GetActiveScene().name.Equals("mainMenu", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("mainMenu");
        }
        else
        {
            Debug.Log("User has initiated exiting the game");
            Application.Quit();
        }
    }

    //Switching SCenes
    public void ChangeScene()
    {

        SaveGame();


        int currentSceneIndex = Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        int nextSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;


        if (!string.IsNullOrEmpty(sceneNames[nextSceneIndex]))
        {
            Debug.Log($"Attempting to load scene: {sceneNames[nextSceneIndex]}");

            if (Application.CanStreamedLevelBeLoaded(sceneNames[nextSceneIndex]))
            {
                if (sceneNames[nextSceneIndex] == "temp_shop")
                {
                    Debug.Log("Ending Day...");
                    Instance.saveData.playerData.currentDay += 1;
                    growAllFlowers();

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

    //Grow All Flowers by 1
    public void growAllFlowers()
    {
        Debug.Log("[GrowAllFlowers]");
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
                flower.flowerData.growthStep += 1;
                flower.ApplyFlowerDataChanges();
            }
        }
        Debug.Log($"Grew FlowerData for {allFlowers.Length} flowers.");
    }

    //Update all Flowers based on an action
    // Example: gameManager.UpdateAllFlowers(flowerData => flowerData.growthStep = 1);
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

    //Change the Current Season and the panel Hue
    public void ChangeSeasonSpell(Season newSeason, Color panelColor)
    {

        seasonPanel = GameObject.Find("SeasonPanel");
        seasonPanelImage = GameObject.Find("SeasonPanel").GetComponent<Image>();
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
