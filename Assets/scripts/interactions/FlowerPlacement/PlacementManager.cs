using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    // Get the placement point at the specific world position
    public PlacementPoint GetPointUnderMouse()
    {
        // Convert mouse position from screen space to world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Create a 2D point using the world space (ignore Z-coordinate)
        Vector2 point2D = new Vector2(worldPosition.x, worldPosition.y);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(point2D);

        foreach (var hitCollider in hitColliders)
        {
            PlacementPoint placementPoint = hitCollider.GetComponent<PlacementPoint>();
            if (placementPoint != null)
            {
                if (placementPoint.IsAvailable())
                {
                    return placementPoint;
                }
                else
                {
                    Debug.LogWarning($"Placement point at {placementPoint.transform.position} is occupied.");
                }
            }
        }

        Debug.LogWarning($"No valid placement point found at: {worldPosition}");
        return null; // No valid point found
    }
}
