using UnityEngine;

public class Flower : MonoBehaviour
{
   
    public int maxGrowthStage = 3;
    public int growthStep = 1;
    public Sprite[] growthSprites;

    
    public Vector3 initialScale = Vector3.one * 0.5f;
    public Vector3 maxScale = Vector3.one * 1.5f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateAppearance();
    }

    public void SetGrowthStep(int step)
    {
        growthStep = Mathf.Clamp(step, 1, maxGrowthStage);
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

    private void UpdateAppearance()
    {
        if (growthSprites != null && growthSprites.Length >= growthStep)
        {
            spriteRenderer.sprite = growthSprites[growthStep - 1];
        }

        float scaleProgress = (float)growthStep / maxGrowthStage;
        transform.localScale = Vector3.Lerp(initialScale, maxScale, scaleProgress);
    }

    public void Interact()
    {
        Grow();
    }
}
