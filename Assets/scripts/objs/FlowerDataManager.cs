using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataModels;

//Handles all data per single flower instance
public class FlowerDataManager : MonoBehaviour
{
    public FlowerConfig flowerConfig; //holds shared data
    public FlowerData flowerData; //holds instance specific data

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        if (flowerData == null)
        {
            flowerData = new FlowerData();
        }
    }

    public void Initialize()
    {
        flowerData.growthStep = flowerConfig.startGrowthStage;
        flowerData.flowerType = flowerConfig.flowerType;
        Debug.Log($"Initialize a flower of type {flowerData.flowerType}");
    }

    public void GrowFlower()
    {
        if (flowerData.growthStep < flowerConfig.maxGrowthStage)
        {
            flowerData.growthStep++;
            Debug.Log("Flower has grown.");
        }
    }
}
