using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the Interaction
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public BoxCollider2D PlayerCollider;
    public Rigidbody2D PlayerRigidBody;

    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnInteract();
        }
    }
}
