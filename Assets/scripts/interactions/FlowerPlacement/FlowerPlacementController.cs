using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determines where on screen flowers will be placed
public class FlowerPlacementController : MonoBehaviour
{
    public FlowerSelectionManager selectionManager;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            PlaceFlowerAtMousePosition();
        }
    }

    private void PlaceFlowerAtMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null)
        {
            PlacementPoint placementPoint = hit.collider.GetComponent<PlacementPoint>();
            if (placementPoint != null)
            {
                FlowerConfig selectedFlower = selectionManager.GetSelectedFlowerConfig();
                if (selectedFlower != null)
                {
                    bool success = placementPoint.TryPlaceFlower(placementPoint.transform.position, selectedFlower);
                    if (success)
                    {
                        Debug.Log("Flower placed successfully.");
                    }
                }
                else
                {
                    Debug.Log("No flower type selected.");
                }
            }
        }
    }
}
