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

    public SaveData saveData = new SaveData();
    public List<GameObject> allFlowers = new List<GameObject>();
      

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
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void Update()
    {

       
    }




    //******************************************* SAVE FUNCTIONALITY CAN GO HERE
    public void SaveGame()
    {
        return;
    }

    public void LoadGame()
    {
        Debug.Log("Loading Game.");
        instatiateDefaultData();

    }

    // Creates a Default SaveData variable for saving to JSON
    public void instatiateDefaultData()
    {
        Debug.Log("Instantiating new data...");
        Instance.saveData = new SaveData();

        Instance.saveData.playerData = new PlayerData()
        {
            goldCount = 500,
            currentDay = 1,
            lastScene = SceneManager.GetSceneByName("morning_shop"),
            spellCast = false
        };

        Instance.saveData.npcData = new List<NPCData>();

        Instance.saveData.allFlowers = new List<GameObject>();

        Instance.saveData.inventoryData = new InventoryData()
        { // This needs to switch out with Item Objects
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
        allFlowers.Add(newFlower);
        Debug.Log("Flower added to the garden.");
    }
    // Find the flower in the list and remove it
    public void RemoveFlower(GameObject flower)
    {
        allFlowers.Remove(flower);
        Debug.Log("Flower removed from the garden.");
    }

}
