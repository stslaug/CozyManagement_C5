using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAnimatorController : MonoBehaviour
{
    public Animator animator; // Link your Animator component here
    public FlowerConfig flowerConfig; // Link your flower ScriptableObject
    private int growthStage; // The flower's current growth stage

    void Start()
    {
        // Initialize growthStage from the ScriptableObject
        growthStage = flowerConfig.startGrowthStage;
    }
    // Call this function whenever the flower grows
    public void Grow()
    {
        // Increment growth stage
        growthStage++;

        // Update the Animator parameter
        animator.SetInteger("growthStage", growthStage);

        // Check if the max growth stage is reached
        if (growthStage == flowerConfig.maxGrowthStage)
        {
            Debug.Log("Max growth reached! Transitioning to fireFlower animation.");
        }
    }
}
