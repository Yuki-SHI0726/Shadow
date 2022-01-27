using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void Start()
    {
        // Set respawn point invisible
        GetComponent<SpriteRenderer>().enabled = false;
    }

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
