using UnityEngine;

public class Flower : MonoBehaviour
{
    public int maxGrowthStage = 10;
    public int growthStep = 1;
    public FlowerData flowerData;

    public Vector3 initialScale = Vector3.one * 0.6f;
    public Vector3 maxScale = Vector3.one * 1.5f;

    private Animator animator;

    private void Awake()
    {
        maxScale = initialScale * 1.5f;
        animator = GetComponent<Animator>();
        UpdateAppearance();
    }

    public void Grow()
    {
        if (growthStep < maxGrowthStage)
        {
            growthStep++;
            UpdateAppearance();
        }
    }

    public void UpdateAppearance()
    {
        float scaleProgress = (float)growthStep / maxGrowthStage;
        transform.localScale = Vector3.Lerp(initialScale, maxScale, scaleProgress);

        // Set the growth step on the Animator to trigger the corresponding animation
        animator.SetInteger("GrowthStep", growthStep);
    }

    public void Interact()
    {
        Grow();
    }
}
