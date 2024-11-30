using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Flower> unplantedFlowers = new List<Flower>();
    public List<Flower> plantedFlowers = new List<Flower>();

    // Adds a new unplanted flower to the inventory
    public void AddUnplantedFlower(FlowerConfig config)
    {
        Flower newFlower = new Flower(config);
        unplantedFlowers.Add(newFlower);
    }

    // Adds a planted flower to the inventory
    public void AddPlantedFlower(Flower flower)
    {
        plantedFlowers.Add(flower);
    }

    // Removes a flower from the inventory
    public void RemoveFlower(Flower flower)
    {
        if (flower.isPlanted)
        {
            plantedFlowers.Remove(flower);
        }
        else
        {
            unplantedFlowers.Remove(flower);
        }
    }

    // Finds a mature flower; you can specify whether to search only unplanted flowers
    public Flower FindMatureFlower(bool onlyPlanted = true)
    {
        List<Flower> searchList = onlyPlanted ? plantedFlowers : unplantedFlowers;
        foreach (var flower in searchList)
        {
            if (flower.IsMature())
            {
                return flower;
            }
        }
        return null;
    }

    // Gets an unplanted flower of a specific type
    public Flower GetUnplantedFlowerOfType(FlowerConfig config)
    {
        foreach (var flower in unplantedFlowers)
        {
            if (flower.flowerConfig == config)
            {
                return flower;
            }
        }
        return null;
    }

    //Find a mature flower of a type (A Mature fire flower)
    public Flower FindPlantedMatureFlowerOfType(FlowerConfig config)
    {
        foreach (var flower in plantedFlowers)
        {
            if (flower.flowerConfig == config && flower.IsMature())
            {
                return flower;
            }
        }
        return null;
    }

    public int GetUnplantedFlowerCount(FlowerConfig config)
    {
        int count = 0;
        foreach (var flower in unplantedFlowers)
        {
            if (flower.flowerConfig == config)
            {
                count++;
            }
        }
        return count;
    }
}
