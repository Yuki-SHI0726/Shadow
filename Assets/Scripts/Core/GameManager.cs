using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    // Levels
    public const int TotalLevelCount = 2;

    // Collecting
    static public int CollectedStarCount = 0;
    static public int TriggerStoryStarCount = 4;        // Trigger story telling with Circe
    static public int StarCountPerLevel = 3;            
    static public int TotalStarCount = StarCountPerLevel * TotalLevelCount;
    static public int CollectedMemoryFragmentCount = 0;
    static public int TotalMemoryFragmentCount = 4;

    // NPC
    public const float kCloseDistanceToInteract = 2.0f;
}
