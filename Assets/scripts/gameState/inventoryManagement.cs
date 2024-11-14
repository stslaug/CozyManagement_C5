using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//Manages flower types, and update counts
public class inventoryManagement : MonoBehaviour
{
    public static GameManager gameManager;
    
    public TextMeshProUGUI FireFlowerText;
    public TextMeshProUGUI WaterFlowerText;
    public TextMeshProUGUI WindFlowerText;
    
    public enum ItemType {
        FireFlower,
        WaterFlower,
        WindFlower
    }

    [System.Serializable]
    public class Item {
        public ItemType itemType;
        public Sprite itemSprite;
        public int count;
        public TextMeshProUGUI itemCountText;
    }

    // Start is called before the first frame update
    public List<Item> items;

    void Start()
    {
        // Initialize item count text for each item
        foreach (Item item in items)
        {
            UpdateItemText(item);
        }
    }

    void Update()
    {
        //
    }

    public void AddItem(ItemType itemType, int num)
    {
        Item item = items.Find(i => i.itemType == itemType);
        if (item != null)
        {
            item.count += num;
            UpdateItemText(item);
        }
        else
        {
            Debug.LogWarning($"Item of type {itemType} not found in the inventory.");
        }
    }

    public void UseItem(ItemType itemType, int num)
    {
        Item item = items.Find(i => i.itemType == itemType);
        if (item != null)
        {
            if (item.count >= num)
            {
                item.count -= num;
                UpdateItemText(item);
            }
            else
            {
                Debug.Log("Not enough items to use.");
            }
        }
        else
        {
            Debug.LogWarning($"Item of type {itemType} not found in the inventory.");
        }
    }

    private void UpdateItemText(Item item)
    {
        if (item.itemCountText != null)
        {
            item.itemCountText.text = item.count.ToString();
        }
        else
        {
            Debug.LogWarning($"Text component for {item.itemType} is not assigned.");
        }
    }
}
