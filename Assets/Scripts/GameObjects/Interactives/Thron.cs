using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sting the player when overlapped
/// </summary>
public class Thron : Interactive
{
    public override void OnInteract(GameObject interactedObject)
    {
        Controller player = interactedObject.GetComponent<Controller>();
        if (player != null)
        {
            player.transform.position = Vector3.zero;
        }
    }
}
