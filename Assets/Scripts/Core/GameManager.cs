using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the game
/// </summary>
public class GameManager : MonoBehaviour
{
    public const int TotalStarsCount = 8;
    public const int TotalMemoryFragmentsCount = 4;
    [HideInInspector] static public int CollectedStarCount = 0;
}
