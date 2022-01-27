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

    /// <summary>
    /// Retrieve the nearest valid spawn point
    /// </summary>
    /// <param name="player">The player</param>
    /// <returns>Vector3 position of the spawn point we found</returns>
    static public Vector3 GetNearestSpawnPoint(GameObject player)
    {
        Vector3 respawnPointToTeleport = player.transform.position;
        float closestDistance = float.MaxValue;

        // Go through each RespawnPoint in the scene
        foreach (RespawnPoint respawnPoint in FindObjectsOfType<RespawnPoint>())
        {
            // Get distance from player to respawn point
            Vector3 playerToRespawnPoint = respawnPoint.transform.position - player.transform.position;
            float distance = playerToRespawnPoint.magnitude;

            // If the current respawn point is the nearest one, and it's on player's left
            if (distance < closestDistance &&
                respawnPoint.transform.position.x <= player.transform.position.x)
            {
                // set value for teleporting to this candidate
                closestDistance = distance;
                respawnPointToTeleport = respawnPoint.transform.position;
            }
        }
        return respawnPointToTeleport;
    }
}
