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
        collectedStarCountText.text = GameManager.CollectedStarCount.ToString();

        // Destroy the collectable object in the scene
        Destroy(gameObject);
    }
}
