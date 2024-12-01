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

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Initialize growthStage from the ScriptableObject
        growthStage = flowerConfig.startGrowthStage;


        // Initialize components
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetInteger("growthStage", growthStage);
    }
    
    void Update()
    {
        // Check if the growth stage has reached maxGrowthStage
        if (growthStage < flowerConfig.maxGrowthStage)
        {
            // Trigger the sprout animation while the growth stage is below max
            animator.SetInteger("growthStage", growthStage);
            
            // Simulate growth over time (this can be tied to time or an external trigger)
            // You can increment the growth stage for the purpose of this example.
            growthStage++;
            
            // Update the growth stage in the Animator
            animator.SetInteger("growthStage", growthStage);
        }
    }



    // Call this function whenever the flower grows
    public void increaseAnimationGrowthStep()
    {
        growthStage++;
        animator.SetInteger("growthStage", growthStage);
        
    }
}
