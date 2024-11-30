using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DataModels;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance;

    private List<string> sceneNames = new List<string> { "mainMenu", "morning_shop", "night_shop", "rooftop_garden" };
    public int currentSceneIndex = 0;

    public SaveData saveData;


    public event Action<int> OnDayChanged;


    private void Start()
    {
       
    }
    void Awake()
    {
        // Singleton Pattern Enforcement
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            saveData = new SaveData();
            instantiateDefaultData();
        }
        else
        {
            Destroy(gameObject);
        }
    }



    void Update()
    {

       
    }




    //******************************************* SAVE FUNCTIONALITY CAN GO HERE
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/savegame.json", json);
        return;
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game Loaded.");
        }
        else
        {
            Debug.LogWarning("Save file not found. Creating new data.");
            instantiateDefaultData();
        }
    }

    // Creates a Default SaveData variable for saving to JSON
    public void instantiateDefaultData()
    {
        Debug.Log("Instantiating new data...");
        saveData = new SaveData();

        saveData.playerData = new PlayerData()
        {
            goldCount = 500,
            currentDay = 1,
            lastScene = "morning_shop", // Use scene name as string
            spellCast = false
        };

        saveData.npcData = new List<NPCData>();

        saveData.allFlowers = new List<FlowerConfig>(); // Initialize as FlowerData list

        saveData.inventoryData = new InventoryData()
        {
            fire_seed = 3,
            wind_seed = 3,
            water_seed = 3,
            fire_extract = 0,
            wind_extract = 0,
            water_extract = 0,
            ice_extract = 0
        };
    }



    // Exit or return to main menu
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

//******************************************* SCENE HANDLING
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //public void LoadSceneAsync() //implement if the scenes take a long time to load

//******************************************* MANAGE ALL INSTANCE DATA
    // Add a new flower instance to the list
    public void AddFlower(GameObject newFlower)
    {
        FlowerConfig t_flower = newFlower.GetComponent<FlowerConfig>();
        saveData.allFlowers.Add(t_flower);
        Debug.Log("Flower added to the garden.");
    }
    // Find the flower in the list and remove it
    public void RemoveFlower(GameObject flower)
    {
        FlowerConfig t_flower = flower.GetComponent<FlowerConfig>();
        saveData.allFlowers.Remove(t_flower);
        Debug.Log("Flower removed from the garden.");
    }

    public SaveData getSaveData()
    { return saveData; }

    public void setSaveData(SaveData data)
    { saveData = data; }

}
