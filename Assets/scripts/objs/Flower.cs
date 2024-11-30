using UnityEngine;
using DataModels;
using System;

public class Flower : MonoBehaviour
{
    [Header("Flower Configuration")]
    public FlowerData flowerData;
    public FlowerConfig config;

    [Header("Growth Settings")]
    public int maxGrowthStage = 10;
    public Vector3 initialScale = Vector3.one * 0.6f;
    public Vector3 maxScale = Vector3.one * 1.5f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Initialize components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Validate FlowerData assignment
        if (flowerData == null)
        {
            Debug.LogError($"FlowerData is not assigned to {gameObject.name}");
            return;
        }

        
        maxScale = initialScale * 1.5f;

       
        UpdateAppearance();
    }

    private void Start()
    {
        
        ApplyFlowerDataChanges();
    }

    private void Update()
    {
        if(animator.GetInteger("GrowthStep") != flowerData.growthStep)
        {
            UpdateAppearance();
        }
    }


    public void UpdateAppearance()
    {
        if (flowerData == null)
            return;

        // Scale the flower based on growth progress
        float scaleProgress = (float)flowerData.growthStep / maxGrowthStage;
        transform.localScale = Vector3.Lerp(initialScale, maxScale, scaleProgress);

        // Update animation parameter if Animator is present
        if (animator != null)
        {
            animator.SetInteger("GrowthStep", flowerData.growthStep);
        }
    }

    public void ApplyFlowerDataChanges()
    {
        UpdateAppearance();
    }


    public void Interact()
    {
       
    }

    public static explicit operator Flower(GameObject v)
    {
        throw new NotImplementedException();
    }
}

