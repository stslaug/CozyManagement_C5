using UnityEngine;
using UnityEngine.SceneManagement;

public class Flower : MonoBehaviour
{
    public int maxGrowthStage = 10;
    public FlowerData flowerData;

    public Vector3 initialScale = Vector3.one * 0.6f;
    public Vector3 maxScale = Vector3.one * 1.5f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (flowerData == null)
        {
            flowerData = new FlowerData
            {
                position = transform.position,
                scene_name = SceneManager.GetActiveScene().name,
                growthStep = 1,
                growthRate = 1f,
                flowerType = "FireFlower",
                canGrowYearRound = true,
                canGrowWinter = false,
                canGrowSummer = true,
                canGrowFall = true,
                canGrowSpring = true,
                needWater = true,
                needSun = true
            };
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
        if (flowerData.growthStep < maxGrowthStage)
        {
            flowerData.growthStep += Mathf.FloorToInt(Time.deltaTime * flowerData.growthRate * 0.1f);
            flowerData.growthStep = Mathf.Clamp(flowerData.growthStep, 1, maxGrowthStage);
            UpdateAppearance();
        }
    }

    public void UpdateAppearance()
    {
        float scaleProgress = (float)flowerData.growthStep / maxGrowthStage;
        transform.localScale = Vector3.Lerp(initialScale, maxScale, scaleProgress);

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
        GameManager.Instance.inventoryData.fireFlowerCount += 1;

        GameManager.Instance.flowerData.RemoveAll(fd =>
            fd.position == flowerData.position &&
            fd.scene_name == flowerData.scene_name &&
            fd.flowerType == flowerData.flowerType
        );

        Destroy(gameObject);

        Debug.Log("Flower harvested and FireFlower seed added to inventory.");
    }
}
