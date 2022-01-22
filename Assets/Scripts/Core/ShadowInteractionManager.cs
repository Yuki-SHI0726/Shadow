using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowInteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject ShadowObject;
    [SerializeField] private ShadowController ShadowController;
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private Controller PlayerController;

    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerController = PlayerObject.GetComponent<Controller>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnInteract();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Follow the movale plane
        if (collision.tag == PlayerController.MovableTag)
        {
            MovablePlane Plane = collision.gameObject.GetComponent<MovablePlane>();
            if (Plane != null)
            {
                ShadowController.ExtensionVelocity = Plane.GetVelocity();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactive Interactive = collision.gameObject.GetComponent<Interactive>();

        if (Interactive != null)
        {
            Interactive.OnExitInteract();
        }

        if (collision.tag == PlayerController.MovableTag)
        {
            // Stop following the movale plane
            ShadowController.ExtensionVelocity = Vector3.zero;
        }
    }

}
