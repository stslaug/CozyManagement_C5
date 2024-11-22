using UnityEngine;
using DataModels;

public class Flower : MonoBehaviour
{
    [Header("Flower Configuration")]
    public FlowerData flowerData;

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

        //// Update color based on current needs
        //if (!flowerData.currentNeeds.Contains(Need.Water) && !flowerData.currentNeeds.Contains(Need.Sunlight))
        //{
        //    //spriteRenderer.color = Color.green; // Healthy
        //}
        //else
        //{
        //    if (flowerData.currentNeeds.Contains(Need.Water) && flowerData.currentNeeds.Contains(Need.Sunlight))
        //        spriteRenderer.color = Color.yellow; // Needs water and sunlight
        //    else if (flowerData.currentNeeds.Contains(Need.Water))
        //        spriteRenderer.color = Color.blue; // Needs water
        //    else if (flowerData.currentNeeds.Contains(Need.Sunlight))
        //        spriteRenderer.color = Color.cyan;// Needs sunlight
        //    else
        //        spriteRenderer.color = Color.red; // Severe condition
        //}
    }

    public void ApplyFlowerDataChanges()
    {
        UpdateAppearance();
    }


    public void Interact()
    {
        Harvest();
    }


    private void Harvest()
    {
        //// Increment the inventory count for the flower type
        //GameManager.Instance.saveData.inventoryData.fireFlowerCount += 1;

        //// Remove the flower's data from the GameManager's list
        //GameManager.Instance.saveData.flowerData.RemoveAll(fd =>
        //    fd.position == flowerData.position &&
        //    fd.scene_name == flowerData.scene_name &&
        //    fd.flowerType == flowerData.flowerType
        //);

        //// Destroy the flower GameObject
        //Destroy(gameObject);

        //Debug.Log($"Flower '{flowerData.flowerType}' harvested and FireFlower seed added to inventory.");
    }
}

