using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sting the player when overlapped
/// </summary>
public class Thorn : Interactive
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
        player.transform.position = GameManager.GetNearestSpawnPoint(player.gameObject);

        // Enable the player's movement & input
        player.Activate();
    }
}
