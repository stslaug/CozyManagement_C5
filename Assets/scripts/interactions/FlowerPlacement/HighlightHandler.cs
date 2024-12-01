using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightHandler : MonoBehaviour
{
    public List<PlacementPoint> placementPoints; // List of all placement points
    public void Start()
    {
        ClearHighlights();
    }

    // Highlight all valid placement points
    public void HighlightValidPoints()
    {
        Debug.Log("Highlights added.");
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

    // Disable all highlights
    public void ClearHighlights()
    {
        foreach (var point in placementPoints)
        {
            point.SetHighlight(false);
        }
        
    }

}


