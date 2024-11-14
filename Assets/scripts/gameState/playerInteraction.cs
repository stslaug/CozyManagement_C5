using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerInteraction : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleFlowerInteraction();
    }

    private void handleFlowerInteraction()
    {
        // Only proceed if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Only proceed if the current scene is "temp_rooftop"
            if (SceneManager.GetActiveScene().name == "temp_rooftop")
            {
                    // Get the mouse position in world coordinates
                    Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector3(
                        Input.mousePosition.x,
                        Input.mousePosition.y,
                        -Camera.main.transform.position.z)); // Ensure correct distance

                    spawnPosition.z = 0; // Set Z to 0 for a 2D game

                    // Cast a ray to check if clicking on an existing flower
                    RaycastHit2D hit = Physics2D.Raycast(spawnPosition, Vector2.zero);
                    if (hit.collider != null)
                    {
                        FlowerManager flower = hit.collider.GetComponent<FlowerManager>();
                        if (flower != null)
                        {
                            flower.Interact(); // Grow the flower
                            return;
                        }
                    }
            }
        }
    }
}
