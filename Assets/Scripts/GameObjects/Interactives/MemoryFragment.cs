using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Memory Fragment object. Collect to compose a full puzzle
/// </summary>
public class MemoryFragment : Interactive
{
    private int m_id = 0;   // The unique Id for this memory fragment. Used for searching the corresponding UI image

    private void OnEnable()
    {
        // Resize BoxCollider2D's size to match SpriteRenderer's size
        Vector3 spriteRendererBoundsSize = GetComponent<SpriteRenderer>().bounds.size;
        GetComponent<BoxCollider2D>().size = spriteRendererBoundsSize;
    }

    void Start()
    {
        // Parse Id from name
        string idString = name.Substring(name.LastIndexOf('_') + 1);
        m_id = int.Parse(idString);
    }

    public override void OnInteract(GameObject interactedObject)
    {
        // Get the corresponding image in UI
        string ImageName = "MemoryFragment_Image_" + m_id.ToString();
        Image image = GameObject.Find(ImageName).GetComponent<Image>();

        ++GameManager.CollectedMemoryFragmentCount;
        if (GameManager.CollectedMemoryFragmentCount == GameManager.TotalMemoryFragmentCount)
        {
            FindObjectOfType<ConversationHandler>().StartConversation(ConversationType.kEnd);
        }

        // Enable it
        image.enabled = true;

        // Destroy the collectable object in the scene
        Destroy(gameObject);
    }
}
