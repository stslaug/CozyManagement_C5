using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

// Implements the winter spell
public class SpellsManager : MonoBehaviour
{   
    public SpriteRenderer backgroundRenderer;
    public Sprite winterBackgroundSprite;
    public FlowerManager flowerManager; // Reference to FlowerManager

    public FlowerConfig iceFlowerConfig;
    public FlowerConfig fireFlowerConfig;
    public FlowerConfig waterFlowerConfig;
     public Inventory inventory;
    private bool spellIsCast = false;

    //chenges the background sprite
    public void SetWinterBiome()
    {
        Debug.Log("Setting Wintertime");
        if (!spellIsCast){
            if(backgroundRenderer != null && winterBackgroundSprite != null)
            {
                backgroundRenderer.sprite = winterBackgroundSprite;
    
            }
            else
            {
                Debug.LogWarning("Background renderer or winter sprite is missing.");
            }

            spellIsCast = true;
            //replace flowers
            ReplaceAndRemoveFlowers(waterFlowerConfig, fireFlowerConfig);
        }
    }

    private void ReplaceAndRemoveFlowers(FlowerConfig waterFlowerConfig, FlowerConfig fireFlowerConfig)
    {
        // Replace all mature water flowers with ice flowers
        ReplaceMatureWaterFlowers(waterFlowerConfig);

        // Remove all mature fire flowers
        RemoveMatureFireFlowers(fireFlowerConfig);
    }

    private void ReplaceMatureWaterFlowers(FlowerConfig waterFlowerConfig)
    {
        if (waterFlowerConfig == null)
        {
            Debug.LogWarning("Water flower config is missing.");
            return;
        }

        foreach (var flower in inventory.plantedFlowers.ToArray()) // Avoid collection modification issues
        {
            if (flower.flowerConfig.flowerType == waterFlowerConfig.flowerType && flower.IsMature())
            {
                // Replace water flower with ice flower
                Vector3 position = flower.position;

                // Use SpawnFlower to create the ice flower
                GameObject iceFlower = flowerManager.SpawnFlower(position, iceFlowerConfig, inventory);
                if (iceFlower != null)
                {
                    Debug.Log($"Replaced a water flower with an ice flower at {position}");
                }

                // Remove the original water flower
                flowerManager.RemoveFlower(flower, inventory); // Pass the Flower data, not flowerData
            }
        }
    }

    private void RemoveMatureFireFlowers(FlowerConfig fireFlowerConfig)
    {
        Debug.Log("Removing Fire Flowers");
        if (fireFlowerConfig == null)
        {
            Debug.LogWarning("Fire flower config is missing.");
            return;
        }

        foreach (var flower in inventory.plantedFlowers.ToArray()) // Avoid collection modification issues
        {
            if (flower.flowerType == fireFlowerConfig.flowerType && flower.IsMature())
            {
                Debug.Log($"Removed a mature fire flower at {flower.position}.");
                flowerManager.RemoveFlower(flower,inventory);
            }
        }
    }
}