using DataModels;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FlowerAnimatorController : MonoBehaviour
{
    public Animator animator; // Link your Animator component here
    public FlowerConfig flowerConfig; // Link your flower ScriptableObject
    private int growthStage; // The flower's current growth stage

    [Header("Growth Settings")]
    public Vector3 initialScale = Vector3.one * 0.6f;
    public Vector3 maxScale = Vector3.one * 1.5f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Initialize growthStage from the ScriptableObject
        growthStage = flowerConfig.startGrowthStage;


        // Initialize components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();



        maxScale = initialScale * 1.5f;
    }


    public void UpdateAppearance()
    {
        // Scale the flower based on growth progress
        float scaleProgress = (float)growthStage / flowerConfig.maxGrowthStage;
        transform.localScale = Vector3.Lerp(initialScale, maxScale, scaleProgress);

        // Update animation parameter if Animator is present
        if (animator != null)
        {
            animator.SetInteger("GrowthStep", growthStage);
        }
    }


    // Call this function whenever the flower grows
    public void increaseAnimationGrowthStep()
    {
        growthStage++;
        animator.SetInteger("growthStage", growthStage);
        UpdateAppearance();
    }
}
