using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DataModels;

public class FlowerManager : MonoBehaviour
{
    public FlowerData flowerData; //holds instance specific data
    public FlowerConfig flowerConfig; //flower type configuration

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
        flowerData.flowerConfig = flowerConfig;
        UpdateAppearance();
    }

    private void Update()
    {
        if (flowerData.growthStep < flowerData.flowerConfig.maxGrowthStage)
        {
            flowerData.growthStep += Mathf.FloorToInt(Time.deltaTime * flowerConfig.growthRate * 0.1f);
            flowerData.growthStep = Mathf.Clamp(flowerData.growthStep, 1, flowerData.flowerConfig.maxGrowthStage);
            UpdateAppearance();
        }
    }

    public void UpdateAppearance()
    {
        if (flowerData == null) return;

        float scaleProgress = (float)flowerData.growthStep / flowerData.flowerConfig.maxGrowthStage;
        transform.localScale = Vector3.Lerp(flowerData.initialScale, flowerData.maxScale, scaleProgress);

        animator.SetInteger("GrowthStep", flowerData.growthStep);

        if (flowerData.needWater && flowerData.needSun)
        {
            spriteRenderer.color = Color.green;
        }
        else if (flowerData.needWater || flowerData.needSun)
        {
            spriteRenderer.color = Color.yellow;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }


    public void Interact()
    {
        Harvest();
    }

    private void Harvest()
    {
        if (flowerData != null && flowerData.flowerConfig.flowerType == "FireFlower")
        {
           GameManager.Instance.inventoryData.fireFlowerCount += 1; 
        }

        GameManager.Instance.RemoveFlower(flowerData);

        Destroy(gameObject);

        Debug.Log("Flower harvested and FireFlower seed added to inventory.");
    }
}
