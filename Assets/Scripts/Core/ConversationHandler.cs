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
    [SerializeField] private Image m_speakerImage = null;
    [SerializeField] private Text m_conversationText = null;
    [SerializeField] private Text m_conversationPromptText = null;
    [SerializeField] private KeyCode m_conversationKey = KeyCode.E;
    [SerializeField] private Sprite m_shadowSprite = null;
    private NPC m_circe = null;

    #region ConversationVariables
    private const char kSpeakerCirce = 'C';
    private const char kSpeakerHardy = 'H';
    private const char kSpeakerShadow = 'S';
    private int m_messageIndex = 0;
    private static ReadOnlyCollection<string> s_kStartConversation = new ReadOnlyCollection<string>(new string[]
    {
        "C嗯呐，你醒了吗？太好啦！",
        "H......",
        "C感觉怎么样？有没有哪里不舒服？",
        "H你……额，你嗯，我，我那个，嗯，为什么会在这里？",
        "C噗，你就只想问这个？",
        "H啊，啊对，我想问这个…",
        "C你可真是个有趣的人呀",
        "H欸，啊，呃那个，那个…我身体没有不舒服的地方，但，但好像什么都想不起来了…",
        "C我想也是，之前你从山上摔下来受了重伤呢，救你的时候魔法出了点问题，现在你的记忆碎片应该散落在森林里了呢，来，进入我的传送门，它能帮助你找回记忆",
    });
    private static ReadOnlyCollection<string> s_kSpecialConversation = new ReadOnlyCollection<string>(new string[]
    {
        "H啊，啊啊啊啊啊",
        "H我想起来了，想起来了，原来，原来我，我竟然是影子吗？！！",
        "H我脚下的，脚下的影子，原来，原来才是，真 正 的 我 吗？",
        "H啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊….",

    });
    private static ReadOnlyCollection<string> s_kEndConversation = new ReadOnlyCollection<string>(new string[]
    {
        "SCirce，他又昏过去了，我们成功了吗？",
        "H嗯呐，别急，让我来看看",
        "S按照之前的合约，他所有的记忆都交给你了，现在你该履行合约，把他变成影子，让我成为Hardy了吧？",
        "H哈哈哈，小影子，你还是这么急躁呢，当然啦，我可是个守信的女巫呢",
        "H嗯呐，不过，你说，我把你们的记忆都交换一下，会发生什么呢？呐！我真期待呢",
    });
    #endregion

    private void Start()
    {
        // Grab circe
        m_circe = FindObjectOfType<NPC>();
        Debug.Assert(m_circe != null);

        // Start conversation coroutine
        StartConversation(ConversationType.kStart);
    }

    /// <summary>
    /// Trigger conversation on different situation
    /// </summary>
    /// <param name="type">The conversation's type to start</param>
    public void StartConversation(ConversationType type)
    {
        // Reset data
        m_messageIndex = 0;

        // Set circe appears nearby
        m_circe.GetComponent<SpriteRenderer>().enabled = true;

        // Get the corresponding conversation content to run
        switch (type)
        {
            case ConversationType.kStart:
                StartCoroutine(ConversationCoroutine(s_kStartConversation));
                break;

            case ConversationType.kSpecial:
                StartCoroutine(ConversationCoroutine(s_kSpecialConversation));
                break;

            case ConversationType.kEnd:
                StartCoroutine(ConversationCoroutine(s_kEndConversation));
                break;
        }
    }

    /// <summary>
    /// Close conversation UI panel
    /// </summary>
    private void DisableUI()
    {
        m_speakerImage.enabled = false;
        m_speakerImage.sprite = null;
        m_conversationImage.enabled = false;
        m_conversationText.enabled = false;
        m_conversationPromptText.enabled = false;
    }

    /// <summary>
    /// Show conversation UI panel
    /// </summary>
    private void EnableUI(ReadOnlyCollection<string> conversation)
    {
        m_speakerImage.enabled = true;
        m_conversationImage.enabled = true;
        m_conversationText.enabled = true;
        m_conversationPromptText.enabled = true;
        UpdateConversationUI(conversation);
    }

    /// <summary>
    /// Coroutine for conversation implementation
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConversationCoroutine(ReadOnlyCollection<string> conversation)
    {
        // Disable player input, enable conversation UI
        GetComponent<Controller>().Deactive();
        EnableUI(conversation);

        while (true)
        {
            if (Input.GetKeyDown(m_conversationKey))
            {
                // If we hit the current conversation's count,
                // means the conversation is over. Exit this while loop
                if (m_messageIndex >= conversation.Count)
                {
                    break;
                }

                UpdateConversationUI(conversation);
            }

            yield return null;
        }

        // Set circe invisible
        m_circe.GetComponent<SpriteRenderer>().enabled = false;

        // Enable player input, disable conversation UI
        GetComponent<Controller>().Activate();
        DisableUI();
    }

    /// <summary>
    /// Parse 
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    private SpeakerData GetSpeakerData(char character)
    {
        if (character == kSpeakerHardy)
        {
            return new SpeakerData(GetComponent<SpriteRenderer>().sprite, GetComponent<Controller>().name);
        }
        else if (character == kSpeakerCirce)
        {
            return new SpeakerData(m_circe.GetComponent<SpriteRenderer>().sprite, m_circe.name);
        }
        else if (character == kSpeakerShadow)
        {
            return new SpeakerData(m_shadowSprite, "Shadow");
        }

        Debug.LogError("Invalid character in conversation");
        return new SpeakerData();
    }

    /// <summary>
    /// Set conversation UI based on current conversation
    /// </summary>
    /// <param name="conversation"></param>
    private void UpdateConversationUI(ReadOnlyCollection<string> conversation)
    {
        SpeakerData speakerData = GetSpeakerData(conversation[m_messageIndex][0]);

        // Image
        m_speakerImage.sprite = speakerData.Sprite;

        // Name
        m_conversationText.text = speakerData.Name;

        // Content
        m_conversationText.text += ": " + conversation[m_messageIndex].Substring(1);

        ++m_messageIndex;
    }
}
