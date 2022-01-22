using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    // Collecting
    [HideInInspector] static public int CollectedStarCount = 0;

    // NPC
    public const float kCloseDistanceToInteract = 2.0f;
}
