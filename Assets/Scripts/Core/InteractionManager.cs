using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager of the Interaction
/// </summary>
public class InteractionManager : MonoBehaviour
{
    private Controller m_playerController = null;

    void Start()
    {
        m_playerController = GetComponentInParent<Controller>();                        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnInteract(m_playerController.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Follow the movale plane
        if (collision.tag == m_playerController.MovableTag)
        {
            MovablePlane Plane = collision.gameObject.GetComponent<MovablePlane>();
            if (Plane != null)
            {
                m_playerController.ExtensionVelocity = Plane.GetVelocity();
            }
        }

        Interactive interactive = collision.gameObject.GetComponent<Interactive>();
        if (interactive != null)
        {
            interactive.OnStayInteract(m_playerController.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnExitInteract(m_playerController.gameObject);
        }

        if (collision.tag == m_playerController.MovableTag)
        {
            // Stop following the movale plane
            m_playerController.ExtensionVelocity = Vector3.zero;          
        }
    }
}
