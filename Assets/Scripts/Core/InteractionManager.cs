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
    [SerializeField] private Controller.Controller PlayerController;

    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerCollider = GetComponent<BoxCollider2D>();
        PlayerController = PlayerObject.GetComponent<Controller.Controller>();
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
        //跟随平台一起滑动
        if (collision.tag == PlayerController.MovableTag)
        {
            MovablePlane Plane = collision.gameObject.GetComponent<MovablePlane>();
            if (Plane != null)
            {
                PlayerController.ExtensionVelocity = Plane.Velocity;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == PlayerController.MovableTag)
        {
            //跳出平台后不再滑动
            PlayerController.ExtensionVelocity = Vector3.zero;          
        }
    }
}
