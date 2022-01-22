using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Manager for the conversation between NPC and the player.
/// This script should be attached to the player
/// </summary>
public class ConversationHandler : MonoBehaviour
{
    [SerializeField] private Image m_conversationImage = null;
    [SerializeField] private Image m_npcImage = null;
    [SerializeField] private Text m_conversationText = null;
    [SerializeField] private Text m_conversationPromptText = null;
    [SerializeField] private KeyCode m_startConversationKey = KeyCode.E;
    [SerializeField] private KeyCode m_endConversationKey = KeyCode.R;
    private List<NPC> m_npcs;     // Stores all the interactivable NPCs
    private int m_messageIndex = 0;
    private static ReadOnlyCollection<string> s_kConversation = new ReadOnlyCollection<string>(new string[]
    {
        "你瞅啥",
        "瞅你咋地",
        "你再瞅一试试",
        "试试就试试",
        "两百多斤的女巫不讲武德",
        "你是来故意找茬的？",
    });

    private void Start()
    {
        m_npcs = new List<NPC>(FindObjectsOfType<NPC>());

        DisableUI();
    }

    private void Update()
    {
        // Grab the player's position
        Vector2 playerPosition2D = new Vector2(transform.position.x, transform.position.y);

        // Loop through all the npcs
        NPC nearbyNPC = null;
        foreach (NPC npc in m_npcs)
        {
            // The npc's position and distance between the script holder
            Vector2 npcPosition2D = new Vector2(npc.transform.position.x, npc.transform.position.y);
            float distance = (playerPosition2D - npcPosition2D).magnitude;

            // If we are close enough to the current npc
            if (distance < GameManager.kCloseDistanceToInteract)
            {
                nearbyNPC = npc;
                EnableUI(npc);
            }
        }

        // Temp Hack
        if (nearbyNPC != null)
        {
            if (Input.GetKeyDown(m_startConversationKey))
            {
                if (m_messageIndex % 2 == 0)
                {
                    m_conversationText.text = name;
                }
                else
                {
                    m_conversationText.text = nearbyNPC.name;
                }
                
                m_conversationText.text += ": " + s_kConversation[m_messageIndex];
                ++m_messageIndex;
                m_messageIndex %= s_kConversation.Count;
            }
        }

        if (nearbyNPC == null)
        {
            DisableUI();
        }
    }

    /// <summary>
    /// Close conversation UI panel
    /// </summary>
    private void DisableUI()
    {
        m_npcImage.enabled = false;
        m_conversationImage.enabled = false;
        m_conversationText.enabled = false;
        m_conversationPromptText.enabled = false;
        m_npcImage.sprite = null;
    }

    /// <summary>
    /// Show conversation UI panel
    /// </summary>
    /// <param name="npc">The npc to interact with</param>
    private void EnableUI(NPC npc)
    {
        m_npcImage.enabled = true;
        m_conversationImage.enabled = true;
        m_conversationText.enabled = true;
        m_conversationPromptText.enabled = true;

        if (m_messageIndex % 2 == 0)
        {
            m_npcImage.sprite = npc.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            m_npcImage.sprite = GetComponent<SpriteRenderer>().sprite;
        }
    }
}
