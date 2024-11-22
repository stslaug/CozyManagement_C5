using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public List<PlacementPoint> placementPoints; // List of all placement points

    // Highlight all valid placement points
    public void HighlightValidPoints()
    {
        Debug.Log("Highlighting valid placement points...");
        foreach (var point in placementPoints)
        {
            if (point.IsAvailable())
            {
                Debug.Log($"Highlight enabled at: {point.transform.position}");
                point.SetHighlight(true);
            }
            else
            {
                Debug.Log($"Highlight skipped for occupied point at: {point.transform.position}");
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
                    Debug.Log($"Placement point detected at {placementPoint.transform.position}, and it is available.");
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
