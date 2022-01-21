using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the Interaction
/// </summary>
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private BoxCollider2D PlayerCollider;
    [SerializeField] private  GameObject PlayerObject;

    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnInteract();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "MovablePlane")
        {
            PlayerObject.transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerObject.transform.SetParent(null);
    }
}
