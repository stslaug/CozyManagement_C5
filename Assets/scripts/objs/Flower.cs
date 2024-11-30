using UnityEngine;
using System;

[Serializable]
public class Flower
{
    [Header("Flower Configuration")]
    public FlowerConfig flowerConfig;

    public Vector3 position;
    public int growthStep;
    public string flowerType;
    public bool isPlanted;

    public Flower(FlowerConfig config)
    {
        flowerConfig = config;
        growthStep = config.startGrowthStage;
        flowerType = config.flowerType;
        isPlanted = false;
    }

    public void Grow()
    {
        if (growthStep < flowerConfig.maxGrowthStage)
        {
            growthStep++;
        }
    }

    public bool IsMature()
    {
        return growthStep >= flowerConfig.maxGrowthStage;
    }
}
