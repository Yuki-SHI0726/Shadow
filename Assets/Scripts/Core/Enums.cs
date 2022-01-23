using UnityEngine;

// Expose enum
[System.Serializable]

public enum InteractiveType
{
    kNpc,
    kMechanism,
    kFlower,
    kStar,
    kMemoryFragment,
    kButton,
    kThorn
}

/// <summary>
/// Conversation type
/// </summary>
public enum ConversationType
{
    kStart,
    kSpecial,
    kEnd
}
