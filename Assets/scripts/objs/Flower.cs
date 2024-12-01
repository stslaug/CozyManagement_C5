using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class Flower
{
    [Header("Flower Configuration")]
    public FlowerConfig flowerConfig;

    public Vector3 position;
    public int growthStep;
    public string flowerType;
    public bool isPlanted;

    public string scene;

    public Flower(FlowerConfig config)
    {
        flowerConfig = config;
        growthStep = config.startGrowthStage;
        flowerType = config.flowerType;
        isPlanted = false;
        scene = SceneManager.GetActiveScene().name;
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
