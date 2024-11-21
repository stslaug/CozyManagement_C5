using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public List<PlacementPoint> placementPoints; // List of all placement points

    // Highlight all valid placement points
    public void HighlightValidPoints()
    {
        foreach (var point in placementPoints)
        {
            if (point.IsAvailable())
            {
                point.SetHighlight(true); // Enable highlight only for available points

            }
            else
            {
                point.SetHighlight(false); // Disable highlight for occupied points
            }
        }
    }

    public void ClearHighlights()
    {
        foreach (var point in placementPoints)
        {
            point.SetHighlight(false); // Disable all highlights
        }

        Debug.Log("All highlights cleared.");
    }

    // Get the placement point at the specific world position
    public PlacementPoint GetPointAtPosition(Vector3 position)
    {
        // Convert world position to a 2D point
    Vector2 point2D = new Vector2(position.x, position.y);

    // Check for colliders at the clicked point
    Collider2D hitCollider = Physics2D.OverlapPoint(point2D);

    if (hitCollider != null)
    {
        PlacementPoint placementPoint = hitCollider.GetComponent<PlacementPoint>();
        if (placementPoint != null && placementPoint.IsAvailable())
        {
            Debug.Log($"Valid placement point found at: {placementPoint.transform.position}");
            return placementPoint;
        }
        else
        {
            Debug.LogWarning("Placement point is either occupied or not valid.");
        }
    }
    else
    {
        Debug.LogWarning($"No collider found at: {position}");
    }

    return null; // No valid point found
    }
}
