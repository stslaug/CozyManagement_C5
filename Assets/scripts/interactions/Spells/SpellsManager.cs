using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class SpellsManager : MonoBehaviour
{   
    public SpriteRenderer backgroundRenderer;
    public Sprite winterBackgroundSprite;

    public GameObject iceFlowerPrefab;
    public FlowerConfig fireFlowerConfig;
    public FlowerConfig waterFlowerConfig;
     public Inventory inventory;
    private bool spellIsCast = false;

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
            if (flower.flowerType == waterFlowerConfig.flowerType && flower.IsMature())
            {
                // Replace water flower with ice flower
                Vector3 position = flower.position;

                GameObject iceFlower = Instantiate(iceFlowerPrefab, position, Quaternion.identity);
                if (iceFlower != null)
                {
                    Debug.Log($"Replaced a water flower with an ice flower at {position}.");

                    // Create a new Flower instance for the ice flower
                    FlowerConfig iceFlowerConfig = iceFlowerPrefab.GetComponent<FlowerDataManager>().flowerConfig;
                    Flower iceFlowerData = new Flower(iceFlowerConfig)
                    {
                        position = position,
                        isPlanted = true
                    };

                    inventory.AddPlantedFlower(iceFlowerData);
                }

                // Remove the original water flower
                inventory.RemoveFlower(flower);
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
                inventory.RemoveFlower(flower);
            }
        }
    }
}