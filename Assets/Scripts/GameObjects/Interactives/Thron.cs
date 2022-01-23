using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sting the player when overlapped
/// </summary>
public class Thron : Interactive
{
    [SerializeField] private int m_blinkCount = 3;        // How many times to blink if the player steps on this thorn
    [SerializeField] private float m_blinkSpeed = 1.0f;   // How fast to blink in seconds

    public override void OnInteract(GameObject interactedObject)
    {
        Controller player = interactedObject.GetComponent<Controller>();
        if (player != null)
        {
            StartCoroutine(KillThePlayer(player));
        }
    }

    private IEnumerator KillThePlayer(Controller player)
    {
        // Disable player's movement & input
        player.Deactive();

        // The player blinks for certain time.
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        Color deltaColor = new Color(0.0f, 0.0f, 0.0f, -m_blinkSpeed * Time.deltaTime);
        int currentBlinkCount = 0;
        while (true)
        {
            // Apply delta color
            playerSpriteRenderer.color += deltaColor;

            // Bounce delta color if hit 0 or 1
            if (playerSpriteRenderer.color.a <= 0.0f)
            {
                deltaColor.a = m_blinkSpeed * Time.deltaTime;
            }
            else if (playerSpriteRenderer.color.a >= 1.0f)
            {
                deltaColor.a = -m_blinkSpeed * Time.deltaTime;
                ++currentBlinkCount;
            }

            // If we hit the blink count for this thorn, exit blink loop
            if (currentBlinkCount >= m_blinkCount)
            {
                break;
            }

            yield return null;
        }

        // Go to the nearest left respawn point
        Vector3 respawnPointToTeleport = Vector3.zero;
        float closestDistance = float.MaxValue;
        foreach (RespawnPoint respawnPoint in FindObjectsOfType<RespawnPoint>())
        {
            // Get distance from player to respawn point
            Vector3 playerToRespawnPoint = respawnPoint.transform.position - player.transform.position;
            float distance = playerToRespawnPoint.magnitude;

            // If the current respawn point is the nearest one, and it's on player's left
            if (distance < closestDistance &&
                respawnPoint.transform.position.x < player.transform.position.x)    
            {
                // set value for teleporting to this candidate
                closestDistance = distance;
                respawnPointToTeleport = respawnPoint.transform.position;
            }
        }
        player.transform.position = respawnPointToTeleport;

        // Enable the player's movement & input
        player.Activate();
    }
}
