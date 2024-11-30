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
            if (gameManager == null)
            {
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            if (gameManager != null)
            {
                if (currFireSeed != gameManager.getSaveData().inventoryData.fire_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("FirePlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.getSaveData().inventoryData.fire_seed.ToString();
                    currFireSeed = gameManager.getSaveData().inventoryData.fire_seed;
                }
                if (currWaterSeed != gameManager.getSaveData().inventoryData.water_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("WaterPlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.getSaveData().inventoryData.water_seed.ToString();
                    currWaterSeed = gameManager.getSaveData().inventoryData.water_seed;
                }
                if (currWindSeed != gameManager.getSaveData().inventoryData.wind_seed)
                {
                    TextMeshProUGUI textTemp = GameObject.Find("WindPlanter_text").GetComponent<TextMeshProUGUI>();
                    textTemp.text = gameManager.getSaveData().inventoryData.wind_seed.ToString();
                    currWindSeed = gameManager.getSaveData().inventoryData.wind_seed;
                }
            }
            
            

        }

    }

    public void AddFire_Seed(int num)
    {
        if (gameManager != null) {
            gameManager.getSaveData().inventoryData.fire_seed += num;
        }
    }

    public void SubFire_Seed(int num)
    {
            if((gameManager.getSaveData().inventoryData.fire_seed - num) < 0)
            {
                Debug.Log("Can't subtract seed. No seeds available!");
                return;
            }
            gameManager.getSaveData().inventoryData.fire_seed -= num;
    }
}
