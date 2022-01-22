using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Star object
/// </summary>
public class Star : Interactive
{
    void Start()
    {
        Type = InteractiveType.kStar;
    }

    public override void OnInteract()
    {
        // Find the CollectedStarCountText
        Text collectedStarCountText = GameObject.Find("CollectedStarCountText").GetComponent<Text>();
        Debug.Assert(collectedStarCountText != null);

        // Increase the CollectedStarCountText's number
        ++GameManager.CollectedStarCount;

        // Update UI text
        int colonIndex = collectedStarCountText.text.IndexOf(':');
        collectedStarCountText.text = collectedStarCountText.text.Remove(colonIndex + 2);
        collectedStarCountText.text += GameManager.CollectedStarCount;

        // Destroy the collectable object in the scene
        Destroy(gameObject);
    }
}
