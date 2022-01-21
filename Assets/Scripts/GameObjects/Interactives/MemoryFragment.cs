using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Memory Fragment object. Collect to compose a full puzzle
/// </summary>
public class MemoryFragment : Interactive
{
    private int m_id = 0;

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
        Debug.Assert(m_id < GameManager.TotalMemoryFragmentsCount);
    }

    public override void OnInteract()
    {
        // Get the corresponding image in UI
        string ImageName = "MemoryFragment_Image_" + m_id.ToString();
        Image image = GameObject.Find(ImageName).GetComponent<Image>();

        // Enable it
        image.enabled = true;

        // Destroy the collectable object in the scene
        Destroy(gameObject);
    }
}
