using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour 
{
    private Inventory inventory;

    [Header("UI Text References")]
    public TextMeshProUGUI FireCount_Text;
    public TextMeshProUGUI WaterCount_Text;
    public TextMeshProUGUI WindCount_Text;

    [Header("Flower Configurations")]
    public FlowerConfig fireConfig;
    public FlowerConfig waterConfig;
    public FlowerConfig windConfig;

    private void Start()
    {
        // Initialize UI with current counts
        try
        {
            inventory = GameObject.Find("inventoryData").GetComponent<Inventory>();
        } catch { }
        UpdateUI();
    }

    private void Update()
    {
        UpdateUpdateUI();
    }

    public void UpdateUI()
    {
        try
        {
            inventory = GameObject.Find("inventoryData").GetComponent<Inventory>();
        }
        catch { }
        if (FireCount_Text != null && fireConfig != null)
        {

            FireCount_Text.text = $"{inventory.GetUnplantedFlowerCount(fireConfig)}";
        }

        if (WaterCount_Text != null && waterConfig != null)
        {
            WaterCount_Text.text = $"{inventory.GetUnplantedFlowerCount(waterConfig)}";
        }

        if (WindCount_Text != null && windConfig != null)
        {
            WindCount_Text.text = $"{inventory.GetUnplantedFlowerCount(windConfig)}";
        }
    }
    public void UpdateUpdateUI()
    {
        try
        {
            inventory = GameObject.Find("inventoryData").GetComponent<Inventory>();
        }
        catch { }
        if (FireCount_Text != null && fireConfig != null)
        {
            if(FireCount_Text.text != ($"{inventory.GetUnplantedFlowerCount(fireConfig)}"))
            {
                FireCount_Text.text = $"{inventory.GetUnplantedFlowerCount(fireConfig)}";
            }
           
        }

        if (WaterCount_Text != null && waterConfig != null)
        {
            if (WaterCount_Text.text != ($"{inventory.GetUnplantedFlowerCount(waterConfig)}"))
            {
                WaterCount_Text.text = $"{inventory.GetUnplantedFlowerCount(waterConfig)}";
            }
        }

        if (WindCount_Text != null && windConfig != null)
        {
            if (WindCount_Text.text != ($"{inventory.GetUnplantedFlowerCount(windConfig)}"))
            {
                WindCount_Text.text = $"{inventory.GetUnplantedFlowerCount(windConfig)}";
            }
          
        }
    }
}
