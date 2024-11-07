using UnityEngine;

public class OnClickAndHover : MonoBehaviour
{
    public Texture2D hoverCursor;  // Assign your cursor texture in the Inspector
    public GameObject flowerPrefab;  // Assign your Flower prefab in the Inspector
    public GameManager gameManager;

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        Cursor.SetCursor(null, hotSpot, cursorMode);
    }

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in world coordinates
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            spawnPosition.z = 0;

            // Cast a ray to check if clicking on an existing flower
            RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Flower flower = hit.collider.GetComponent<Flower>();
                if (flower != null)
                {
                    flower.Interact();  // Grow the flower
                    return;
                }
            }

            // Instantiate the flower prefab at the click position if no flower was clicked
            GameObject newFlower = Instantiate(flowerPrefab, spawnPosition, Quaternion.identity);
            Flower flowerComponent = newFlower.GetComponent<Flower>();

            // Add the new flower's data to the GameManager's flowerData list
            if (flowerComponent != null)
            {
                gameManager.flowerData.Add(flowerComponent.flowerData);
            }

            Debug.Log("Flower spawned at: " + spawnPosition);
        }
    }
}
