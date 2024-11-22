using DataModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManagement : MonoBehaviour
{
    private GameManager gameManager;


    public int currFireSeed;
    public int currWaterSeed;
    public int currWindSeed;
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        currFireSeed = 0;
        currWaterSeed = 0;
        currWindSeed = 0;
    }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("rooftop_garden"))
        {

                if (currFireSeed != gameManager.saveData.inventoryData.fire_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("FirePlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.saveData.inventoryData.fire_seed.ToString();
                    currFireSeed = gameManager.saveData.inventoryData.fire_seed;
                }
                if (currWaterSeed != gameManager.saveData.inventoryData.water_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("WaterPlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.saveData.inventoryData.water_seed.ToString();
                    currWaterSeed = gameManager.saveData.inventoryData.water_seed;
                }
                if (currWindSeed != gameManager.saveData.inventoryData.wind_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("WindPlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.saveData.inventoryData.wind_seed.ToString();
                    currWindSeed = gameManager.saveData.inventoryData.wind_seed;
                }
            

        }

    }

    public void addFireFlower(int num)
    {
        gameManager.saveData.inventoryData.fire_seed += num;
    }
}
