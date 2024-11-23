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
                point.SetHighlight(true);
            }
            else
            {
                point.SetHighlight(false);
            }
        }
    }

    public void ClearHighlights()
    {
        foreach (var point in placementPoints)
        {
            point.SetHighlight(false); // Disable all highlights
        }
    }

    // Get the placement point at the specific world position
    public PlacementPoint GetPointAtPosition(Vector3 position)
    {
        Vector2 point2D = new Vector2(position.x, position.y);
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

        Debug.LogWarning($"No valid placement point found at: {position}");
        return null; // No valid point found
    }
}
