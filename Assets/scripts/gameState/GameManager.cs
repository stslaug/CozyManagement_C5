using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DataModels;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static CurrencyTracker currencyTracker;
    public static DaysTracker daysTracker;
    public GameObject pauseMenu;
    string saveDirectory;
    private string[] sceneNames = { "temp_rooftop", "temp_shop" };
    public PlayerData playerData;
    public SaveData saveData;
    public List<NPCData> npcData;
    public InventoryData inventoryData;

    public GameObject flowerPrefab;
    public List<FlowerData> allFlowerData = new List<FlowerData>();

    // Reference to the season panel and its Image component
    private GameObject seasonPanel;
    
    

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
        if (string.IsNullOrEmpty(saveDirectory))
        {
            saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        }

        string filePath = Path.Combine(saveDirectory, "save.json");

        saveData = null;
        playerData = null;
        npcData = null;
        flowerPrefab = null;
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
                allFlowerData = loadedSaveData.allFlowerData;
                inventoryData = loadedSaveData.inventoryData;

                if (playerData == null)
                {
                    Debug.LogError("playerData is null in loaded SaveData.");
                    return;
                }

                Debug.Log("Game loaded successfully. Switching scenes...");

                if (!string.IsNullOrEmpty(playerData.lastScene))
                {
                    Debug.Log("Attempting to load scene: " + playerData.lastScene);

                    if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
                    {
                        SceneManager.sceneLoaded += OnSceneLoaded;
                        SceneManager.LoadScene(playerData.lastScene);
                    }
                    else
                    {
                        Debug.LogError("Scene '" + playerData.lastScene + "' cannot be loaded.");
                    }
                }
                else
                {
                    Debug.LogError("playerData.lastScene is null or empty.");
                }
            }
            else
            {
                Debug.LogWarning("Save file not found. Returning default player data.");

                saveData = new SaveData();
                playerData = new PlayerData();
                npcData = new List<NPCData>();
                allFlowerData = new List<FlowerData>();

                playerData.creationDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                playerData.lastScene = "temp_shop";

                saveData.playerData = playerData;
                saveData.playerData.currentDay = 1;
                saveData.npcData = npcData;
                saveData.allFlowerData = allFlowerData;
                saveData.inventoryData = inventoryData;

                Debug.Log("Loading default scene: " + playerData.lastScene);

                if (Application.CanStreamedLevelBeLoaded(playerData.lastScene))
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadScene(playerData.lastScene);
                }
                else
                {
                    Debug.LogError("Default scene '" + playerData.lastScene + "' cannot be loaded.");
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading save file from {filePath}: {e.Message}\n{e.StackTrace}");
        }
    }

    private GameObject yeti;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (allFlowerData != null)
        {
            foreach (FlowerData data in allFlowerData)
            {
                if (data.scene_name == scene.name)
                {
                    if (flowerPrefab != null)
                    {
                        GameObject flowerObject = Instantiate(flowerPrefab, data.position, Quaternion.identity);
                        FlowerManager flower = flowerObject.GetComponent<FlowerManager>();

                        if (flower != null)
                        {
                            flower.flowerData = data;
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
            Debug.LogError("CurrencyTracker component not found.");
        }

        // Update Days tracker
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
            Debug.LogError("daysTracker component not found.");
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
                Debug.LogWarning("PauseMenu GameObject not found.");
            }
        }

        // Activate Yeti on day 2 in temp_shop
        if (scene.name == "temp_shop" && Instance.playerData.currentDay == 2)
        {
            Debug.Log("In yeti activation");
            ActivateYeti();
        }

        // Unsubscribe to prevent multiple calls
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void ActivateYeti()
    {
        GameObject yeti = GameObject.Find("Yeti_Request");

        if (yeti == null)
        {
            GameObject playerUI = GameObject.Find("Player_UI");

            if (playerUI != null)
            {
                Transform yetiTransform = playerUI.transform.Find("Yeti_Request");

                if (yetiTransform != null)
                {
                    yeti = yetiTransform.gameObject;
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

        if (yeti != null)
        {
            yeti.SetActive(true);
            Debug.Log("Yeti has appeared!");
        }
        else
        {
            Debug.LogError("Yeti GameObject not found.");
        }
    }

    // Save the game
    public void SaveGame()
    {
        string filePath = Path.Combine(saveDirectory, "save.json");

        if (saveData == null)
            saveData = new SaveData();

        if (saveData.playerData == null)
            saveData.playerData = new PlayerData();

        if (saveData.npcData == null)
            saveData.npcData = new List<NPCData>();

        if (saveData.allFlowerData == null)
            saveData.allFlowerData = new List<FlowerData>();

        if (allFlowerData == null)
            allFlowerData = new List<FlowerData>();

        if (inventoryData == null)
            inventoryData = new InventoryData();

        playerData.lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        playerData.lastScene = SceneManager.GetActiveScene().name;

        string currentSceneName = SceneManager.GetActiveScene().name;
        allFlowerData.RemoveAll(data => data.scene_name == currentSceneName);

        FlowerManager[] flowers = FindObjectsOfType<FlowerManager>();
        foreach (FlowerManager flower in flowers)
        {
            FlowerData data = new FlowerData
            {
                position = flower.transform.position,
                scene_name = currentSceneName,
                growthStep = flower.flowerData.growthStep,
                flowerConfig = flower.flowerData.flowerConfig,
                needWater = flower.flowerData.needWater,
                needSun = flower.flowerData.needSun
            };

            allFlowerData.Add(data);
        }

        saveData.playerData = playerData;
        saveData.npcData = npcData;
        saveData.allFlowerData = allFlowerData;
        saveData.inventoryData = inventoryData;

        string json = JsonUtility.ToJson(saveData);

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

    // Delete the save file
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

    // Exit or return to main menu
    public void ExitFunction()
    {
        if (SceneManager.GetSceneByName("mainMenu") != SceneManager.GetActiveScene())
        {
            Debug.Log("Loading Main Menu");
            SceneManager.LoadScene("mainMenu");

            saveData = null;
            playerData = null;
            npcData = null;
            allFlowerData = null;
            inventoryData = null;
        }
        else
        {
            Debug.Log("User has initiated exiting the game");
            Application.Quit();
        }
    }

    // Change to the next scene
    public void ChangeScene()
    {
        int currentSceneIndex = Array.IndexOf(sceneNames, SceneManager.GetActiveScene().name);
        int nextSceneIndex = (currentSceneIndex + 1) % sceneNames.Length;

        Instance.SaveGame();

        if (!string.IsNullOrEmpty(sceneNames[nextSceneIndex]))
        {
            Debug.Log("Attempting to load scene: " + sceneNames[nextSceneIndex]);

            if (Application.CanStreamedLevelBeLoaded(sceneNames[nextSceneIndex]))
            {
                if (sceneNames[nextSceneIndex] == "temp_shop")
                {
                    Debug.Log("Ending Day...");
                    Instance.playerData.currentDay += 1;

                    DaysTracker.Instance.UpdateDayDisplay();
                }
                else
                {
                    Debug.Log("Scene '" + sceneNames[nextSceneIndex] + "' loaded.");
                }

                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.LoadScene(sceneNames[nextSceneIndex]);
            }
            else
            {
                Debug.LogError("Scene '" + sceneNames[nextSceneIndex] + "' cannot be loaded.");
            }
        }
        else
        {
            Debug.LogError("sceneNames[nextSceneIndex] is null or empty.");
        }
    }

    void SpawnFlower(Vector3 position, FlowerConfig flowerConfig)
    {
        //Create a flower instance
        FlowerData data = new FlowerData()
        {
            flowerConfig = flowerConfig,
            position = position,
            growthStep = 1,  // Initial growth step
            needWater = true,
            needSun = true
        };

        allFlowerData.Add(data);
    }
    /*
     * ex.
     * GameManager.Instance.UpdateAllFlowers(flowerData => flowerData.growthStep = 1);
    */
    public void UpdateAllFlowers(Action<FlowerData> updateAction)
    {
        if (updateAction == null)
        {
            Debug.LogError("Update action is null.");
            return;
        }

        FlowerManager[] allFlowers = FindObjectsOfType<FlowerManager>();

        if (allFlowers.Length == 0)
        {
            Debug.LogWarning("No Flower objects found in the scene.");
            return;
        }

        foreach (FlowerManager flower in allFlowers)
        {
            if (flower != null && flower.flowerData != null)
            {
                updateAction(flower.flowerData);
                flower.UpdateAppearance();
            }
        }

        Debug.Log($"Updated FlowerData for {allFlowers.Length} flowers.");
    }

    public void AddFlowerData(FlowerData flowerData)
    {
        allFlowerData.Add(flowerData);
        Debug.Log("Flower added to the garden.");
    }

    public void RemoveFlower(FlowerData flower)
    {
        // Find the flower in the list and remove it
        allFlowerData.Remove(flower);
        Debug.Log("Flower removed from the garden.");
    }

}
