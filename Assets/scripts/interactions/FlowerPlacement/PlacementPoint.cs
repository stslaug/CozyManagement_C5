using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ensures flower placement can only happen in specified regions.
public class PlacementPoint : MonoBehaviour
{
    public SpriteRenderer highlightSprite; // Sprite renderer to show the highlight
    private bool isOccupied = false; // Track if the point is occupied

    // Enable or disable highlighting for this point
    public void SetHighlight(bool isVisible)
    {
        if (highlightSprite != null)
        {
            highlightSprite.enabled = isVisible; // Only enable if not occupied
        }
    }

    // Mark the point as occupied
    public void OccupyPoint()
    {
        isOccupied = true;
    }

    public void FreePoint()
    {
        isOccupied = false;
    }

    public bool IsAvailable()
    {
        return !isOccupied;
    }
}
