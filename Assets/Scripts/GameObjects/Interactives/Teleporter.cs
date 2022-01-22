using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Teleport the player to other positions
/// </summary>
public class Teleporter : Interactive
{
    [SerializeField] private Vector3 m_destination = Vector3.zero;

    public override void OnInteract()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    public override void OnStayInteract()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }
    }

    public override void OnExitInteract()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }
}
