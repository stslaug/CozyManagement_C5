using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManagement : MonoBehaviour
{
    private GameManager gameManager;

    public int fireFlowerCount;
    public int waterFlowerCount; 
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        fireFlowerCount = 0;
    }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        fireFlowerCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(fireFlowerCount != gameManager.saveData.inventoryData.fireFlowerCount)
        {
            fireFlowerCount = gameManager.saveData.inventoryData.fireFlowerCount;
            TextMeshProUGUI FireFlowerText = GameObject.Find("FirePlanter_text").GetComponent<TextMeshProUGUI>();
            FireFlowerText.text = fireFlowerCount.ToString();
        }
        
    }

    public void addFireFlower(int num)
    {
        gameManager.saveData.inventoryData.fireFlowerCount += num;
    }
}
