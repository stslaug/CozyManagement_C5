using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures flower placement can only happen in specified regions.
public class PlacementPoint : MonoBehaviour
{
    public SpriteRenderer highlightSprite; // Sprite renderer to show the highlight
    private bool isOccupied = false; // Track if the point is occupied

    // Enable or disable highlighting for this point
    public void SetHighlight(bool isActive)
{
    if (highlightSprite != null)
    {
        highlightSprite.enabled = isActive && !isOccupied; // Only enable if not occupied
    }
}

    // Mark the point as occupied
    public void OccupyPoint()
    {
        isOccupied = true;
        Debug.Log($"Point at {transform.position} is now occupied.");
        SetHighlight(false);
    }

    public void FreePoint()
    {
        isOccupied = false;
        Debug.Log($"Point at {transform.position} is now free.");
    }

    public bool IsAvailable()
    {
        Debug.Log($"Point at {transform.position} is " + (isOccupied ? "not available" : "available") + ".");
        return !isOccupied;
    }
}
